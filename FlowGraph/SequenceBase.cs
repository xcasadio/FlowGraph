using System.Xml;
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

    protected SequenceBase(string? name)
    {
        Name = name;
        Id = _newId++;
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

    public virtual void Load(XmlNode node)
    {
        Id = int.Parse(node.Attributes["id"].Value);
        if (_newId <= Id) _newId = Id + 1;
        Name = node.Attributes["name"].Value;
        Description = node.Attributes["description"].Value;

        foreach (XmlNode nodeNode in node.SelectNodes("NodeList/Node"))
        {
            var versionNode = int.Parse(nodeNode.Attributes["version"].Value);

            var seqNode = SequenceNode.CreateNodeFromXml(nodeNode);

            if (seqNode != null)
            {
                AddNode(seqNode);
            }
            else
            {
                throw new InvalidOperationException($"Can't create SequenceNode from xml id={nodeNode.Attributes["id"].Value}");
            }
        }
    }

    internal void ResolveNodesLinks(XmlNode node)
    {
        if (node == null) throw new ArgumentNullException("XmlNode");

        var connectionListNode = node.SelectSingleNode("ConnectionList");

        foreach (var sequenceNode in SequenceNodes)
        {
            sequenceNode.Value.ResolveLinks(connectionListNode, this);
        }
    }

    public virtual void Save(XmlNode node)
    {
        const int version = 1;

        XmlNode graphNode = node.OwnerDocument.CreateElement("Graph");
        node.AppendChild(graphNode);

        graphNode.AddAttribute("version", version.ToString());
        graphNode.AddAttribute("id", Id.ToString());
        graphNode.AddAttribute("name", Name);
        graphNode.AddAttribute("description", Description);

        //save all nodes
        XmlNode nodeList = node.OwnerDocument.CreateElement("NodeList");
        graphNode.AppendChild(nodeList);
        //save all connections
        XmlNode connectionList = node.OwnerDocument.CreateElement("ConnectionList");
        graphNode.AppendChild(connectionList);

        foreach (var pair in SequenceNodes)
        {
            XmlNode nodeNode = node.OwnerDocument.CreateElement("Node");
            nodeList.AppendChild(nodeNode);
            pair.Value.Save(nodeNode);
            pair.Value.SaveConnections(connectionList);
        }
    }
}