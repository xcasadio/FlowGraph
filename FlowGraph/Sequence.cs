using Newtonsoft.Json.Linq;
using FlowGraph.Nodes;

namespace FlowGraph;

public class Sequence : SequenceBase
{
    public Sequence()
    {

    }

    public Sequence(string? name = null)
        : base(name)
    {
    }

    public Sequence(Sequence sequence)
        : base(sequence)
    {
    }

    public bool ContainsEventNodeWithType(Type type)
    {
        return SequenceNodes.Any(pair => pair.Value is EventNode && pair.Value.GetType() == type);
    }

    public override void Save(JObject node)
    {
        node["type"] = "sequence";
        base.Save(node);
    }

    public Sequence Clone()
    {
        var sequence = new Sequence(this);
        return sequence;
    }
}