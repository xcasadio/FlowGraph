using FlowGraph.Nodes;
using Newtonsoft.Json.Linq;

namespace CustomNode;

public interface ISequenceNodeFactory
{
    SequenceNode? CreateNode(JObject jObject);
}