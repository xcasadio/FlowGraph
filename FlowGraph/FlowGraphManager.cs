using Newtonsoft.Json.Linq;
using FlowGraph.Logger;

namespace FlowGraph;

public class FlowGraphManager
{
    public Sequence Sequence { get; private set; } = new("sequence");
    public List<SequenceFunction> Functions { get; } = new();

    //macros ?
    //variables

    public void Load(JObject node)
    {
        Sequence.Load(node);

        //functions sequence

        Sequence.ResolveNodesLinks(node);

        //foreach (var function in Functions)
        //{
        //    ResolveLinks(node, function);
        //}
    }

    public void Save(JObject node)
    {
        Sequence.Save(node);
        /*
        foreach (var seq in Functions)
        {
            seq.Save(list);
        }*/
    }

    public FlowGraphManager Clone()
    {
        var flowGraphManager = new FlowGraphManager();

        flowGraphManager.Sequence = Sequence.Clone();

        foreach (var sequenceFunction in Functions)
        {
            flowGraphManager.Functions.Add(sequenceFunction.Copy());
        }

        return flowGraphManager;
    }
}