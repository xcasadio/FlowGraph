using Newtonsoft.Json.Linq;
using FlowGraph.Nodes;
using FlowGraph.Process;

namespace FlowGraph;

public class SequenceBase
{
    static int _newId;

    protected readonly Dictionary<int, SequenceNode> SequenceNodes = new();

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int Id { get; private set; }

    public IEnumerable<SequenceNode> Nodes => SequenceNodes.Values.ToArray();

    public int NodeCount => SequenceNodes.Values.Count;

    protected SequenceBase(string? name = null)
    {
        Name = name;
        Id = _newId++;
    }

    protected SequenceBase(SequenceBase sequenceBase)
    {
        Name = sequenceBase.Name;
        Description = sequenceBase.Description;
        Id = _newId++;

        foreach (var pair in sequenceBase.SequenceNodes)
        {
            SequenceNodes.Add(pair.Key, pair.Value.Copy());
        }
    }

    public SequenceNode GetNodeById(int id)
    {
        return SequenceNodes[id];
    }

    public void AddNode(SequenceNode node)
    {
        SequenceNodes.Add(node.Id, node);
    }

    public void RemoveNode(SequenceNode node)
    {
        SequenceNodes.Remove(node.Id);
    }

    public void RemoveAllNodes()
    {
        SequenceNodes.Clear();
    }

    public void AllocateAllVariables(MemoryStack memoryStack)
    {
        foreach (var varNode in SequenceNodes.Select(pair => pair.Value).OfType<VariableNode>())
        {
            varNode.Allocate(memoryStack);
        }
    }

    public void ResetNodes()
    {
        foreach (var pair in SequenceNodes)
        {
            pair.Value.Reset();
        }
    }

    public void OnEvent(ProcessingContext context, Type type, int index, object? param)
    {
        foreach (var eventNode in SequenceNodes
                     .Select(pair => pair.Value as EventNode)
                     .Where(node => node != null && node.GetType() == type))
        {
            eventNode.Triggered(context, index, param);
        }
    }

    public virtual void Load(JObject node)
    {
        RemoveAllNodes();

        Id = node["id"].Value<int>();
        if (_newId <= Id)
        {
            _newId = Id + 1;
        }

        Name = node["name"].Value<string>();
        Description = node["description"].Value<string>();

        foreach (var nodeNode in node["nodes"])
        {
            AddNode(SequenceNode.CreateNodeFromJson((JObject)nodeNode));
        }
    }

    internal void ResolveNodesLinks(JObject node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        foreach (var sequenceNode in SequenceNodes)
        {
            var selectToken = node.SelectToken($"nodes[?(@.id=={sequenceNode.Key})]");
            sequenceNode.Value.ResolveLinks((JObject)selectToken, this);
        }
    }

    public virtual void Save(JObject node)
    {
        node["id"] = Id;
        node["name"] = Name;
        node["description"] = Description;

        var jsonArrayNodes = new JArray();
        foreach (var pair in SequenceNodes)
        {
            var nodeSlot = new JObject();
            pair.Value.Save(nodeSlot);
            jsonArrayNodes.Add(nodeSlot);

            pair.Value.SaveConnections(nodeSlot);
        }

        node["nodes"] = jsonArrayNodes;
    }
}