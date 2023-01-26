using FlowGraph.Node;

namespace FlowGraph.Process
{
    public class ProcessingContext
    {
        public enum ProcessingContextState
        {
            Running,
            Pause,
            Stop
        }

        private static int _freeCallId;

        public event EventHandler Finished;

        private readonly List<ProcessingContextStep> _nextExecutions = new();
        private readonly List<ProcessingContextStep> _executed = new();

        public IEnumerable<ProcessingContextStep> Executed => _executed;

        public int CallId
        {
            get;
        }

        public ProcessingContextState State
        {
            get;
            set;
        }

        public SequenceBase SequenceBase
        {
            get;
        }

        private MemoryStackFrameManager MemoryStackFrame
        {
            get;
        }

        public MemoryStack? CurrentFrame => MemoryStackFrame.CurrentFrame;

        public SequenceNode? CurrentNode
        {
            get;
            set;
        }

        public bool IsOnError
        {
            get;
            set;
        }

        public ProcessingContext CurrentProcessingContext => Child == null ? this : Child.CurrentProcessingContext;

        public ProcessingContext? Parent
        {
            get;
        }

        public ProcessingContext? Child
        {
            get;
            private set;
        }

        public ProcessingContext(SequenceBase seq, MemoryStackFrameManager stack, ProcessingContext? parent = null)
        {
            State = ProcessingContextState.Stop;

            _freeCallId++;
            CallId = _freeCallId;

            Parent = parent;
            SequenceBase = seq;
            MemoryStackFrame = stack;
        }

        public ProcessingContext PushNewContext()
        {
            Child = new ProcessingContext(SequenceBase, MemoryStackFrame, this);
            return Child;
        }

        public ProcessingContextStep PopStep()
        {
            ProcessingContextStep step = null;

            if (CurrentProcessingContext._nextExecutions.Count > 0)
            {
                step = CurrentProcessingContext._nextExecutions[0];
                CurrentProcessingContext._nextExecutions.RemoveAt(0);
                CurrentProcessingContext._executed.Add(step);
            }
            else
            {
                CurrentProcessingContext.Finished?.Invoke(CurrentProcessingContext, EventArgs.Empty);

                if (CurrentProcessingContext.Parent != null)
                {
                    var c = CurrentProcessingContext;
                    CurrentProcessingContext.Parent.Child = null;
                    // Call Parent.PopStep() else the execution will stop because step is null
                    step = c.Parent.PopStep();
                }
            }

            return step;
        }

        public ProcessingContextStep RegisterNextExecution(NodeSlot slot)
        {
            if (slot.Node is ActionNode == false)
            {
                throw new InvalidOperationException("ProcessingContext.RegisterNextExecution() : the node is not an ActionNode");
            }

            var step = new ProcessingContextStep(SequenceBase, CurrentFrame, slot);
            CurrentProcessingContext._nextExecutions.Add(step);
            return step;
        }

        public void RemoveExecution(ProcessingContext context, ProcessingContextStep step)
        {
            context._nextExecutions.Remove(step);
        }

        public void RegisterNextSequence(SequenceBase? seq, Type eventType, object? para)
        {
            CurrentProcessingContext.MemoryStackFrame.AddStackFrame();
            seq.AllocateAllVariables(CurrentProcessingContext.MemoryStackFrame.CurrentFrame);
            seq.OnEvent(this, eventType, 0, para);
        }

        public void RemoveLastSequence()
        {
            CurrentProcessingContext.MemoryStackFrame.RemoveStackFrame();
            //TODO remove child
        }
    }
}
