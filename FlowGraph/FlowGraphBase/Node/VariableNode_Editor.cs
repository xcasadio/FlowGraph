using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowGraphBase.Logger;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    public enum VariableControlType
    {
        Numeric,
        Selectable,
        Checkable,
        Text,
        ReadOnly,
        Custom
    }


    /// <summary>
    /// 
    /// </summary>
    public abstract partial class VariableNode
        : SequenceNode
    {
		#region Fields
		
		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public override NodeType NodeType
        {
            get { return NodeType.Variable; }
        }

        /// <summary>
        /// 
        /// </summary>
        public NodeSlot VariableSlot
        {
            get { return m_Slots[0]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Title
        {
            get { return ""; }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public VariableNode()
            : base()
        {
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Save(XmlNode node_)
        {
            base.Save(node_);

            XmlNode valueNode = node_.OwnerDocument.CreateElement("Value");
            node_.AppendChild(valueNode);
            SaveValue(valueNode);
        }

        /// <summary>
        /// Load the value from XmlNode
        /// </summary>
        /// <param name="node_"></param>
        protected abstract object LoadValue(XmlNode node_);

        /// <summary>
        /// Save the value into a XmlNode
        /// </summary>
        /// <param name="node_"></param>
        protected abstract void SaveValue(XmlNode node_);

		#endregion //Methods
    }
}
