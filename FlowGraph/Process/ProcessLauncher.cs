using System.ComponentModel;
using FlowGraph.Logger;
using FlowGraph.Node;

namespace FlowGraph.Process
{
    public class ProcessLauncher : INotifyPropertyChanged
    {
        private static ProcessLauncher? _singleton;

        public static ProcessLauncher Instance => _singleton ??= new ProcessLauncher();

        private readonly List<ProcessingContext> _callStacks = new();
        private int _currentCallStackIndex;

        private volatile bool _updateOnlyOneStep;
        private volatile bool _isOnPause;
        private volatile bool _mustStop;

        private ProcessingContextStep _lastExecution;
        private SequenceState _state = SequenceState.Stop;

        private BackgroundWorker _bgWorker;
        private volatile bool _isClosing;

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

        public IEnumerable<ProcessingContext> CallStack => _callStacks;

        private ProcessLauncher()
        {

        }

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

        public void StopLoop()
        {
            _isClosing = true;
            _mustStop = true;
        }

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

        public void NextStep()
        {
            State = SequenceState.Pause;
            _updateOnlyOneStep = true;
            _isOnPause = true;
        }

        public void Pause()
        {
            State = SequenceState.Pause;
            _isOnPause = true;
        }

        public void Stop()
        {
            State = SequenceState.Stop;
            _mustStop = true;
            _isOnPause = false;
            _updateOnlyOneStep = false;
        }

        public void OnGlobalEvent(Type eventType, int index, object para)
        {
            var seqList = new List<Sequence>();

            foreach (var c in _callStacks)
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

        public void LaunchSequence(SequenceBase seq, Type eventType, int index, object para)
        {
            var stackFrames = new MemoryStackFrameManager();
            //stackFrames.Clear();
            stackFrames.AddStackFrame(); // 1st frame
            seq.AllocateAllVariables(stackFrames.CurrentFrame);
            var processContext = new ProcessingContext(seq, stackFrames);
            seq.OnEvent(processContext, eventType, index, para);
            _callStacks.Add(processContext);

            Resume();
        }

        private void ProcessLoop(bool sleep = true)
        {
            var processing = true;

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

            foreach (var c in _callStacks)
            {
                c.SequenceBase.ResetNodes();
            }
        }

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

                var context = _callStacks[_currentCallStackIndex];
                var contextStep = context.PopStep();

                if (contextStep == null)
                {
                    _callStacks.Remove(context);
                    context.SequenceBase.ResetNodes();
                }
                else
                {
                    var node = contextStep.Slot.Node as ActionNode;
                    _lastExecution = contextStep;

                    var info = node.Activate(context, contextStep.Slot);
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

        private int GetNumberOfActiveProcess()
        {
            var count = 0;

            foreach (var c in _callStacks)
            {
                if (c.IsOnError == false)
                {
                    count++;
                }
            }

            return count;
        }

        private bool SetNextProcess()
        {
            for (var i = _currentCallStackIndex + 1; i < _callStacks.Count; i++)
            {
                if (_callStacks[i].IsOnError == false)
                {
                    _currentCallStackIndex = i;
                    return true;
                }
            }

            for (var i = 0; i <= _currentCallStackIndex && i < _callStacks.Count; i++)
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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
