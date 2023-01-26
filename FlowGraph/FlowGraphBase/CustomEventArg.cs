using System;

namespace FlowGraphBase
{
    public enum FunctionSlotChangedType
    {
        Added,
        Removed
    }

    public class FunctionSlotChangedEventArg : EventArgs
    {
        public FunctionSlotChangedType Type
        {
            get;
        }

        public SequenceFunctionSlot FunctionSlot
        {
            get;
        }

        public FunctionSlotChangedEventArg(FunctionSlotChangedType type, SequenceFunctionSlot slot)
        {
            Type = type;
            FunctionSlot = slot;
        }
    }
}
