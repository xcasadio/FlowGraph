using FlowGraphBase.Node;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// Snapshot of the execution
    /// </summary>
    public class ProcessingContextStep
    {
        /// <summary>
        /// Gets
        /// </summary>
	    public SequenceBase SequenceBase { get; }

        /// <summary>
        /// Gets
        /// </summary>
	    public MemoryStack MemoryStack { get; }

        /// <summary>
        /// Gets
        /// </summary>
	    public NodeSlot Slot { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq_"></param>
        /// <param name="stack_"></param>
        /// <param name="slot_"></param>
        public ProcessingContextStep(SequenceBase seq, MemoryStack stack, NodeSlot slot)
        {
            SequenceBase = seq;
            MemoryStack = stack;
            Slot = slot;
        }
    }
}
