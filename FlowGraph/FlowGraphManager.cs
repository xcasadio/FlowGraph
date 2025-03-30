using System.Data;
using Newtonsoft.Json.Linq;
using FlowGraph.Logger;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlowGraph;

public class FlowGraphManager
{
    public Sequence Sequence { get; private set; } = new("sequence");
    public List<SequenceFunction> Functions { get; } = new();

    //macros ?
    //variables

    public void Load(JObject node)
    {
        var sequenceObject = (JObject)node["sequence"];

        Sequence.Load(sequenceObject);

        //functions sequence
        Functions.Clear();

        Sequence.ResolveNodesLinks(sequenceObject);

        //foreach (var function in Functions)
        //{
        //    ResolveLinks(node, function);
        //}
    }

    public void Save(JObject node)
    {
        JObject sequenceObject = new();
        Sequence.Save(sequenceObject);
        node.Add("sequence", sequenceObject);
        /*
        foreach (var seq in Functions)
        {
            seq.Save(list);
        }*/
    }

    public FlowGraphManager Clone()
    {
        var flowGraphManager = new FlowGraphManager
        {
            Sequence = Sequence.Clone()
        };

        foreach (var sequenceFunction in Functions)
        {
            flowGraphManager.Functions.Add(sequenceFunction.Copy());
        }

        return flowGraphManager;
    }
}