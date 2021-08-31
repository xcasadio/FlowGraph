using System;

namespace FlowGraphBase
{
    public enum FunctionSlotChangedType
    {
        Added,
        Removed
    }

    /// <summary>
    /// 
    /// </summary>
    public class FunctionSlotChangedEventArg : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public FunctionSlotChangedType Type
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public SequenceFunctionSlot FunctionSlot
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        /// <param name="slot_"></param>
        public FunctionSlotChangedEventArg(FunctionSlotChangedType type_, SequenceFunctionSlot slot_)
        {
            Type = type_;
            FunctionSlot = slot_;
        }
    }
}
