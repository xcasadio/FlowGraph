using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase.Node;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessingContext
    {
        #region nested struct

        /// <summary>
        /// 
        /// </summary>
        public enum ProcessingContextState
        {
            Running,
            Pause,
            Stop
        }

        #endregion // nested struct

		#region Fields

        static private int m_FreeCallID = 0;

        public event EventHandler Finished;

        private List<ProcessingContextStep> m_NextExecutions = new List<ProcessingContextStep>();
        private List<ProcessingContextStep> m_Executed = new List<ProcessingContextStep>();

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// Gets all step already executed
        /// </summary>
        public IEnumerable<ProcessingContextStep> Executed
        {
            get { return m_Executed; }
        }

        /// <summary>
        /// Gets the ID of the context
        /// </summary>
        public int CallID
        {
            get;
            private set;
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
            private set;
        }

        /// <summary>
        /// Gets the memory manager
        /// </summary>
        private MemoryStackFrameManager MemoryStackFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public MemoryStack CurrentFrame
        {
            get { return MemoryStackFrame.CurrentFrame; }
        }
        
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
        public ProcessingContext CurrentProcessingContext
        {
            get
            {
                return Child == null ? this : Child.CurrentProcessingContext;
            }
        }

        /// <summary>
        /// Gets
        /// </summary>
        public ProcessingContext Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public ProcessingContext Child
        {
            get;
            private set;
        }

		#endregion //Properties
	
		#region Constructors
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq_"></param>
        /// <param name="stack_"></param>
        /// <param name="parent_"></param>
        public ProcessingContext(SequenceBase seq_, MemoryStackFrameManager stack_, ProcessingContext parent_ = null)
        {
            State = ProcessingContextState.Stop;

            m_FreeCallID++;
            CallID = m_FreeCallID;

            Parent = parent_;
            SequenceBase = seq_;
            MemoryStackFrame = stack_;
        }

		#endregion //Constructors
	
		#region Methods

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

            if (CurrentProcessingContext.m_NextExecutions.Count > 0)
            {
                step = CurrentProcessingContext.m_NextExecutions[0];
                CurrentProcessingContext.m_NextExecutions.RemoveAt(0);
                CurrentProcessingContext.m_Executed.Add(step);
            }
            else
            {
                if (CurrentProcessingContext.Finished != null)
                {
                    CurrentProcessingContext.Finished(CurrentProcessingContext, EventArgs.Empty);
                }

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
        public ProcessingContextStep RegisterNextExecution(NodeSlot slot_)
        {
            if (slot_.Node is ActionNode == false)
            {
                throw new InvalidOperationException("ProcessingContext.RegisterNextExecution() : the node is not an ActionNode");
            }

            ProcessingContextStep step = new ProcessingContextStep(SequenceBase, CurrentFrame, slot_);
            CurrentProcessingContext.m_NextExecutions.Add(step);
            return step;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="step_"></param>
        public void RemoveExecution(ProcessingContext context_, ProcessingContextStep step_)
        {
            context_.m_NextExecutions.Remove(step_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq_"></param>
        /// <param name="eventType_"></param>
        public void RegisterNextSequence(SequenceBase seq_, Type eventType_, object param_)
        {
            CurrentProcessingContext.MemoryStackFrame.AddStackFrame();
            seq_.AllocateAllVariables(CurrentProcessingContext.MemoryStackFrame.CurrentFrame);
            seq_.OnEvent(this, eventType_, 0, param_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveLastSequence()
        {
            CurrentProcessingContext.MemoryStackFrame.RemoveStackFrame();
            //TODO remove child
        }

		#endregion //Methods
    }
}
