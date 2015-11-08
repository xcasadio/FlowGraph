using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowGraphBase.Node;
using System.ComponentModel;
using FlowGraphBase.Process;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public enum SequenceState
    {
        Running,
        Pause,
        Stop
    }

    /// <summary>
    /// 
    /// </summary>
    public class Sequence : SequenceBase
    {
        #region Fields

        public const string XmlAttributeTypeValue = "Sequence";

        #endregion // Fields

        #region Properties

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public Sequence(string name_)
            : base(name_)
        { }

        /// <summary>
        /// 
        /// </summary>
        public Sequence(XmlNode node_)
            : base(node_)
        { }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        public bool ContainsEventNodeWithType(Type type_)
        {
            foreach (var pair in m_SequenceNodes)
            {
                if (pair.Value is EventNode
                    && pair.Value.GetType().Equals(type_) == true)
                {
                    return true;
                }
            }

            return false;
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Save(XmlNode node_)
        {
            base.Save(node_);

            XmlNode graphNode = node_.SelectSingleNode("Graph[@id='" + ID + "']");
            graphNode.AddAttribute("type", XmlAttributeTypeValue);
        }

        #endregion // Persistence

        #endregion // Methods
    }
}
