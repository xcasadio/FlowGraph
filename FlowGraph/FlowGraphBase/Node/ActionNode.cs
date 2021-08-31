using System.Xml;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    public abstract
#if EDITOR
    partial
#endif
    class ActionNode
        : SequenceNode
    {
        #region nested struct 
 
        /// <summary>
        /// 
        /// </summary>
        public enum LogicState
        {
            Ok,
            Warning,
            Error
        }

        /// <summary>
        /// 
        /// </summary>
        public struct ProcessingInfo
        {
            public LogicState State;
            public string ErrorMessage;
        }

        #endregion // nested struct

        #region Fields

        private string _errorMessage = "";
        private ProcessingInfo _state;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public ProcessingInfo State
        {
            get => _state;
            private set 
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            internal set 
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        protected ActionNode(XmlNode node)
            : base(node)
        {

        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            SlotFlag = SlotAvailableFlag.All;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public ProcessingInfo Activate(ProcessingContext context, NodeSlot slot)
        {
            State = ActivateLogic(context, slot);
            return State;
        }

        /// <summary>
        /// Methods call when the node is activated.
        /// The other node connected to the input connector has activated his output link.
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="slot_"></param>
        /// <returns></returns>
        public abstract ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_);

		#endregion //Methods        
    }
}
