using FlowGraph.Node;

namespace FlowGraph.Process
{
    public class ProcessingContextStep
    {
        public SequenceBase SequenceBase { get; }

        public MemoryStack MemoryStack { get; }

        public NodeSlot Slot { get; }

        public ProcessingContextStep(SequenceBase seq, MemoryStack stack, NodeSlot slot)
        {
            SequenceBase = seq;
            MemoryStack = stack;
            Slot = slot;
        }
    }
}
