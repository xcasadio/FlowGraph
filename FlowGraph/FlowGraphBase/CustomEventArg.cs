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
		#region Fields
		
		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public FunctionSlotChangedType Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public SequenceFunctionSlot FunctionSlot
        {
            get;
            private set;
        }

		#endregion //Properties
	
		#region Constructors

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

		#endregion //Constructors
	
		#region Methods
		
		#endregion //Methods
    }
}
