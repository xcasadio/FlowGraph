using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            OK,
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

        private string m_ErrorMessage = "";
        private ProcessingInfo m_State;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public ProcessingInfo State
        {
            get { return m_State; }
            private set 
            {
                m_State = value;
                OnPropertyChanged("State");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage
        {
            get { return m_ErrorMessage; }
            internal set 
            {
                m_ErrorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ActionNode(XmlNode node_)
            : base(node_)
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
        /// <param name="context_"></param>
        /// <param name="slot_"></param>
        /// <returns></returns>
        public ProcessingInfo Activate(ProcessingContext context_, NodeSlot slot_)
        {
            State = ActivateLogic(context_, slot_);
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
