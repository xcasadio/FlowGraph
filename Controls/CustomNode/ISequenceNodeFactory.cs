using System.Xml;
using FlowGraph.Nodes;

namespace CustomNode;

public interface ISequenceNodeFactory
{
    SequenceNode? CreateNode(XmlNode node);
}