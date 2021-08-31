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
        private static ProcessLauncher _singleton;

        /// <summary>
        /// Gets
        /// </summary>
        public static ProcessLauncher Instance => _singleton ?? (_singleton = new ProcessLauncher());

        private readonly List<ProcessingContext> _callStacks = new List<ProcessingContext>();
        private int _currentCallStackIndex;

        private volatile bool _updateOnlyOneStep;
        private volatile bool _isOnPause;
        private volatile bool _mustStop;

        private ProcessingContextStep _lastExecution;
        private SequenceState _state = SequenceState.Stop;

        private BackgroundWorker _bgWorker;
        private volatile bool _isClosing;

        /// <summary>
        /// Gets
        /// </summary>
        public SequenceState State
        {
            get => _state;
            private set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// Gets
        /// </summary>
        public IEnumerable<ProcessingContext> CallStack => _callStacks;

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
            if (_bgWorker == null)
            {
                _bgWorker = new BackgroundWorker();
                _bgWorker.DoWork += (sender1, e1) =>
                {
                    try
                    {
                        while (_isClosing == false)
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

                _bgWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopLoop()
        {
            _isClosing = true;
            _mustStop = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resume()
        {
            State = SequenceState.Running;
            _updateOnlyOneStep = false;
            _isOnPause = false;
            _mustStop = false;

            if (_lastExecution != null)
            {
                _lastExecution.Slot.Node.IsProcessing = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NextStep()
        {
            State = SequenceState.Pause;
            _updateOnlyOneStep = true;
            _isOnPause = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Pause()
        {
            State = SequenceState.Pause;
            _isOnPause = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            State = SequenceState.Stop;
            _mustStop = true;
            _isOnPause = false;
            _updateOnlyOneStep = false;
        }

        /// <summary>
        /// Trigger the event node with the type eventType_ only in running Sequence
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="index"></param>
        /// <param name="para"></param>
        public void OnGlobalEvent(Type eventType, int index, object para)
        {
            List<Sequence> seqList = new List<Sequence>();

            foreach (ProcessingContext c in _callStacks)
            {
                if (c.SequenceBase is Sequence seq)
                {
                    if (seq.ContainsEventNodeWithType(eventType)
                        && seqList.Contains(seq) == false)
                    {
                        seqList.Add(seq);
                        seq.OnEvent(c, eventType, index, para);
                    }
                }
            }

            //Resume();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="eventType"></param>
        /// <param name="index"></param>
        /// <param name="para"></param>
        public void LaunchSequence(SequenceBase seq, Type eventType, int index, object para)
        {
            MemoryStackFrameManager stackFrames = new MemoryStackFrameManager();
            //stackFrames.Clear();
            stackFrames.AddStackFrame(); // 1st frame
            seq.AllocateAllVariables(stackFrames.CurrentFrame);
            ProcessingContext processContext = new ProcessingContext(seq, stackFrames);
            seq.OnEvent(processContext, eventType, index, para);
            _callStacks.Add(processContext);

            Resume();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessLoop(bool sleep = true)
        {
            bool processing = true;

            while (processing
                && _mustStop == false)
            {
                if (_isOnPause == false)
                {
                    processing = DoOneStep();

                    if (processing)
                    {
                        State = SequenceState.Running;
                    }
                }
                else if (_updateOnlyOneStep)
                {
                    _updateOnlyOneStep = false;
                    processing = DoOneStep();

                    if (processing)
                    {
                        State = SequenceState.Pause;
                    }
                }

                if (sleep)
                {
                    // Do sleep else all WPF bindings will block the UI thread
                    // 5ms else the UI is not enough responsive
                    Thread.Sleep(5);
                }
            };

            State = SequenceState.Stop;
            _lastExecution = null;

            foreach (ProcessingContext c in _callStacks)
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
                    if (_lastExecution != null)
                    {
                        _lastExecution.Slot.Node.IsProcessing = false;
                    }
                }

                ProcessingContext context = _callStacks[_currentCallStackIndex];
                ProcessingContextStep contextStep = context.PopStep();

                if (contextStep == null)
                {
                    _callStacks.Remove(context);
                    context.SequenceBase.ResetNodes();
                }
                else
                {
                    ActionNode node = contextStep.Slot.Node as ActionNode;
                    _lastExecution = contextStep;

                    ActionNode.ProcessingInfo info = node.Activate(context, contextStep.Slot);
                    node.ErrorMessage = info.ErrorMessage;

                    if (info.State == ActionNode.LogicState.Error)
                    {
                        context.IsOnError = true;
                    }

                    if (State == SequenceState.Pause)
                    {
                        _lastExecution.Slot.Node.IsProcessing = true;
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

            foreach (var c in _callStacks)
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
            for (int i = _currentCallStackIndex + 1; i < _callStacks.Count; i++)
            {
                if (_callStacks[i].IsOnError == false)
                {
                    _currentCallStackIndex = i;
                    return true;
                }
            }

            for (int i = 0; i <= _currentCallStackIndex && i < _callStacks.Count; i++)
            {
                if (_callStacks[i].IsOnError == false)
                {
                    _currentCallStackIndex = i;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
