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
        public const string XmlAttributeTypeValue = "Sequence";

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ContainsEventNodeWithType(Type type)
        {
            return SequenceNodes.Any(pair => pair.Value is EventNode && pair.Value.GetType() == type);
        }

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
    }
}
