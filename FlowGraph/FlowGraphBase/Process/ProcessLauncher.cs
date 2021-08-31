using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using FlowGraphBase.Logger;
using FlowGraphBase.Node;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessLauncher : INotifyPropertyChanged
    {
        private static ProcessLauncher _Singleton;

        /// <summary>
        /// Gets
        /// </summary>
        public static ProcessLauncher Instance
        {
            get
            {
                if (_Singleton == null)
                {
                    _Singleton = new ProcessLauncher();
                }

                return _Singleton;
            }
        }

        private readonly List<ProcessingContext> _CallStacks = new List<ProcessingContext>();
        private int _CurrentCallStackIndex;

        private volatile bool _UpdateOnlyOneStep;
        private volatile bool _IsOnPause;
        private volatile bool _MustStop;

        private ProcessingContextStep _LastExecution;
        private SequenceState _State = SequenceState.Stop;

        private BackgroundWorker _BGWorker;
        private volatile bool _IsClosing;

        /// <summary>
        /// Gets
        /// </summary>
        public SequenceState State
        {
            get => _State;
            private set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// Gets
        /// </summary>
        public IEnumerable<ProcessingContext> CallStack => _CallStacks;

        /// <summary>
        /// 
        /// </summary>
        private ProcessLauncher()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void StartLoop()
        {
            if (_BGWorker == null)
            {
                _BGWorker = new BackgroundWorker();
                _BGWorker.DoWork += (sender1, e1) =>
                {
                    try
                    {
                        while (_IsClosing == false)
                        {
                            ProcessLoop();
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Instance.WriteException(ex);
                    }
                };

                _BGWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopLoop()
        {
            _IsClosing = true;
            _MustStop = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resume()
        {
            State = SequenceState.Running;
            _UpdateOnlyOneStep = false;
            _IsOnPause = false;
            _MustStop = false;

            if (_LastExecution != null)
            {
                _LastExecution.Slot.Node.IsProcessing = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NextStep()
        {
            State = SequenceState.Pause;
            _UpdateOnlyOneStep = true;
            _IsOnPause = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Pause()
        {
            State = SequenceState.Pause;
            _IsOnPause = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            State = SequenceState.Stop;
            _MustStop = true;
            _IsOnPause = false;
            _UpdateOnlyOneStep = false;
        }

        /// <summary>
        /// Trigger the event node with the type eventType_ only in running Sequence
        /// </summary>
        /// <param name="eventType_"></param>
        /// <param name="index_"></param>
        /// <param name="para_"></param>
        public void OnGlobalEvent(Type eventType_, int index_, object para_)
        {
            List<Sequence> seqList = new List<Sequence>();

            foreach (ProcessingContext c in _CallStacks)
            {
                if (c.SequenceBase is Sequence)
                {
                    Sequence seq = c.SequenceBase as Sequence;

                    if (seq.ContainsEventNodeWithType(eventType_)
                        && seqList.Contains(seq) == false)
                    {
                        seqList.Add(seq);
                        seq.OnEvent(c, eventType_, index_, para_);
                    }
                }
            }

            //Resume();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq_"></param>
        /// <param name="eventType_"></param>
        /// <param name="index_"></param>
        /// <param name="para_"></param>
        public void LaunchSequence(SequenceBase seq_, Type eventType_, int index_, object para_)
        {
            MemoryStackFrameManager stackFrames = new MemoryStackFrameManager();
            //stackFrames.Clear();
            stackFrames.AddStackFrame(); // 1st frame
            seq_.AllocateAllVariables(stackFrames.CurrentFrame);
            ProcessingContext processContext = new ProcessingContext(seq_, stackFrames);
            seq_.OnEvent(processContext, eventType_, index_, para_);
            _CallStacks.Add(processContext);

            Resume();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessLoop(bool sleep_ = true)
        {
            bool processing = true;

            while (processing
                && _MustStop == false)
            {
                if (_IsOnPause == false)
                {
                    processing = DoOneStep();

                    if (processing)
                    {
                        State = SequenceState.Running;
                    }
                }
                else if (_UpdateOnlyOneStep)
                {
                    _UpdateOnlyOneStep = false;
                    processing = DoOneStep();

                    if (processing)
                    {
                        State = SequenceState.Pause;
                    }
                }

                if (sleep_)
                {
                    // Do sleep else all WPF bindings will block the UI thread
                    // 5ms else the UI is not enough responsive
                    Thread.Sleep(5);
                }
            };

            State = SequenceState.Stop;
            _LastExecution = null;

            foreach (ProcessingContext c in _CallStacks)
            {
                c.SequenceBase.ResetNodes();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if there is at least one node to be activated else returns false</returns>
        public bool DoOneStep()
        {
            if (GetNumberOfActiveProcess() > 0)
            {
                if (State == SequenceState.Pause)
                {
                    if (_LastExecution != null)
                    {
                        _LastExecution.Slot.Node.IsProcessing = false;
                    }
                }

                ProcessingContext context = _CallStacks[_CurrentCallStackIndex];
                ProcessingContextStep contextStep = context.PopStep();

                if (contextStep == null)
                {
                    _CallStacks.Remove(context);
                    context.SequenceBase.ResetNodes();
                }
                else
                {
                    ActionNode node = contextStep.Slot.Node as ActionNode;
                    _LastExecution = contextStep;

                    ActionNode.ProcessingInfo info = node.Activate(context, contextStep.Slot);
                    node.ErrorMessage = info.ErrorMessage;

                    if (info.State == ActionNode.LogicState.Error)
                    {
                        context.IsOnError = true;
                    }

                    if (State == SequenceState.Pause)
                    {
                        _LastExecution.Slot.Node.IsProcessing = true;
                    }
                }
            }

            return SetNextProcess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfActiveProcess()
        {
            int count = 0;

            foreach (var c in _CallStacks)
            {
                if (c.IsOnError == false)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool SetNextProcess()
        {
            for (int i = _CurrentCallStackIndex + 1; i < _CallStacks.Count; i++)
            {
                if (_CallStacks[i].IsOnError == false)
                {
                    _CurrentCallStackIndex = i;
                    return true;
                }
            }

            for (int i = 0; i <= _CurrentCallStackIndex && i < _CallStacks.Count; i++)
            {
                if (_CallStacks[i].IsOnError == false)
                {
                    _CurrentCallStackIndex = i;
                    return true;
                }
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
