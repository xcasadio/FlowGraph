using FlowGraphBase.Node;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// Snapshot of the execution
    /// </summary>
    public class ProcessingContextStep
    {
		#region Fields

        #endregion //Fields
	
		#region Properties
		
        /// <summary>
        /// Gets
        /// </summary>
	    public SequenceBase SequenceBase { get; private set; }

        /// <summary>
        /// Gets
        /// </summary>
	    public MemoryStack MemoryStack { get; private set; }

        /// <summary>
        /// Gets
        /// </summary>
	    public NodeSlot Slot { get; private set; }

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
