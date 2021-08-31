using FlowGraphBase.Node;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// Snapshot of the execution
    /// </summary>
    public class ProcessingContextStep
    {
		#region Fields

        private SequenceBase m_SequenceBase;
        private MemoryStack m_MemoryStack;
        private NodeSlot m_Slot;

		#endregion //Fields
	
		#region Properties
		
        /// <summary>
        /// Gets
        /// </summary>
	    public SequenceBase SequenceBase
	    {
		    get => m_SequenceBase;
            private set => m_SequenceBase = value;
        }

        /// <summary>
        /// Gets
        /// </summary>
	    public MemoryStack MemoryStack
	    {
		    get => m_MemoryStack;
            private set => m_MemoryStack = value;
        }
        
        /// <summary>
        /// Gets
        /// </summary>
	    public NodeSlot Slot
	    {
            get => m_Slot;
            private set => m_Slot = value;
        }

		#endregion //Properties
	
		#region Constructors
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq_"></param>
        /// <param name="stack_"></param>
        /// <param name="slot_"></param>
        public ProcessingContextStep(SequenceBase seq_, MemoryStack stack_, NodeSlot slot_)
        {
            SequenceBase = seq_;
            MemoryStack = stack_;
            Slot = slot_;
        }

		#endregion //Constructors
	
		#region Methods
		
		#endregion //Methods
    }
}
