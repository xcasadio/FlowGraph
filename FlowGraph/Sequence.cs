using System.Xml;
using FlowGraph.Nodes;

namespace FlowGraph;

public class Sequence : SequenceBase
{
    public const string? XmlAttributeTypeValue = "Sequence";

    public Sequence(string? name)
        : base(name)
    {

    }

    public bool ContainsEventNodeWithType(Type type)
    {
        return SequenceNodes.Any(pair => pair.Value is EventNode && pair.Value.GetType() == type);
    }

    public override void Save(XmlNode node)
    {
        base.Save(node);

        var graphNode = node.SelectSingleNode("Graph[@id='" + Id + "']");
        graphNode?.AddAttribute("type", XmlAttributeTypeValue);
    }
}