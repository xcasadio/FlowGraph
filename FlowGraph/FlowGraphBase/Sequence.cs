using System;
using System.Linq;
using System.Xml;
using FlowGraphBase.Node;

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
        public Sequence(string name)
            : base(name)
        { }

        /// <summary>
        /// 
        /// </summary>
        public Sequence(XmlNode node)
            : base(node)
        { }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ContainsEventNodeWithType(Type type)
        {
            return SequenceNodes.Any(pair => pair.Value is EventNode && pair.Value.GetType() == type);
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public override void Save(XmlNode node)
        {
            base.Save(node);

            XmlNode graphNode = node.SelectSingleNode("Graph[@id='" + Id + "']");
            graphNode.AddAttribute("type", XmlAttributeTypeValue);
        }

        #endregion // Persistence

        #endregion // Methods
    }
}
