using System;
using System.Collections.Generic;
using FlowGraphBase.Node;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessingContext
    {
        /// <summary>
        /// 
        /// </summary>
        public enum ProcessingContextState
        {
            Running,
            Pause,
            Stop
        }

        private static int _freeCallId;

        public event EventHandler Finished;

        private readonly List<ProcessingContextStep> _nextExecutions = new List<ProcessingContextStep>();
        private readonly List<ProcessingContextStep> _executed = new List<ProcessingContextStep>();

        /// <summary>
        /// Gets all step already executed
        /// </summary>
        public IEnumerable<ProcessingContextStep> Executed => _executed;

        /// <summary>
        /// Gets the Id of the context
        /// </summary>
        public int CallId
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public ProcessingContextState State
        {
            get;
            set;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public SequenceBase SequenceBase
        {
            get;
        }

        /// <summary>
        /// Gets the memory manager
        /// </summary>
        private MemoryStackFrameManager MemoryStackFrame
        {
            get;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public MemoryStack CurrentFrame => MemoryStackFrame.CurrentFrame;

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public SequenceNode CurrentNode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public bool IsOnError
        {
            get;
            set;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public ProcessingContext CurrentProcessingContext => Child == null ? this : Child.CurrentProcessingContext;

        /// <summary>
        /// Gets
        /// </summary>
        public ProcessingContext Parent
        {
            get;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public ProcessingContext Child
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="stack"></param>
        /// <param name="parent"></param>
        public ProcessingContext(SequenceBase seq, MemoryStackFrameManager stack, ProcessingContext parent = null)
        {
            State = ProcessingContextState.Stop;

            _freeCallId++;
            CallId = _freeCallId;

            Parent = parent;
            SequenceBase = seq;
            MemoryStackFrame = stack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ProcessingContext PushNewContext()
        {
            Child = new ProcessingContext(SequenceBase, MemoryStackFrame, this);
            return Child;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                    ProcessingContext c = CurrentProcessingContext;
                    CurrentProcessingContext.Parent.Child = null;
                    // Call Parent.PopStep() else the execution will stop because step is null
                    step = c.Parent.PopStep(); 
                }
            }

            return step;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ProcessingContextStep RegisterNextExecution(NodeSlot slot)
        {
            if (slot.Node is ActionNode == false)
            {
                throw new InvalidOperationException("ProcessingContext.RegisterNextExecution() : the node is not an ActionNode");
            }

            ProcessingContextStep step = new ProcessingContextStep(SequenceBase, CurrentFrame, slot);
            CurrentProcessingContext._nextExecutions.Add(step);
            return step;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="step"></param>
        public void RemoveExecution(ProcessingContext context, ProcessingContextStep step)
        {
            context._nextExecutions.Remove(step);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="eventType"></param>
        public void RegisterNextSequence(SequenceBase seq, Type eventType, object para)
        {
            CurrentProcessingContext.MemoryStackFrame.AddStackFrame();
            seq.AllocateAllVariables(CurrentProcessingContext.MemoryStackFrame.CurrentFrame);
            seq.OnEvent(this, eventType, 0, para);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveLastSequence()
        {
            CurrentProcessingContext.MemoryStackFrame.RemoveStackFrame();
            //TODO remove child
        }
    }
}
