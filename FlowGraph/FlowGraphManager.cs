using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Xml;
using FlowGraph.Logger;
using Logger;


namespace FlowGraph;

public class FlowGraphManager
{
    public Sequence Sequence { get; } = new("sequence");
    public List<SequenceFunction> Functions { get; } = new();

    //macros ?
    //variables

    public void Load(JObject node)
    {
        try
        {
            Sequence.Load(node);

            //functions sequence

            Sequence.ResolveNodesLinks(node);

            //foreach (var function in Functions)
            //{
            //    ResolveLinks(node, function);
            //}
        }
        catch (Exception ex)
        {
            LogManager.Instance.WriteException(ex);
        }
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
}