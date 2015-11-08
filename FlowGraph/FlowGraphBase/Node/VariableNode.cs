using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowGraphBase.Logger;
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
    class VariableNode
        : SequenceNode
    {
		#region Fields

        MemoryStackItem m_MemoryItem;

		#endregion //Fields

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public VariableNode(XmlNode node_)
            : base(node_)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            SlotFlag = SlotAvailableFlag.DefaultFlagVariable;
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public abstract object Value
        {
            get;
            set;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStack_"></param>
        public void Allocate(MemoryStack memoryStack_)
        {
            // TODO : create function => object abstract CopyValue(object val_) ?????
            // do this only to copy the value
            VariableNode clone = (VariableNode)CopyImpl();
            m_MemoryItem = memoryStack_.Allocate(ID, clone.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStack_"></param>
        public void Deallocate(MemoryStack memoryStack_)
        {
            memoryStack_.Deallocate(ID);
            m_MemoryItem = null;
        }

        #endregion // Methods
    }
}
