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

		#endregion //Fields

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        protected VariableNode(XmlNode node)
            : base(node)
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
        /// <param name="memoryStack"></param>
        public void Allocate(MemoryStack memoryStack)
        {
            // TODO : create function => object abstract CopyValue(object val_) ?????
            // do this only to copy the value
            VariableNode clone = (VariableNode)CopyImpl();
            memoryStack.Allocate(Id, clone.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStack"></param>
        public void Deallocate(MemoryStack memoryStack)
        {
            memoryStack.Deallocate(Id);
        }

        #endregion // Methods
    }
}
