using System.Xml;
using FlowGraph.Node;

namespace CustomNode;

public interface ISequenceNodeFactory
{
    SequenceNode? CreateNode(XmlNode node);
}