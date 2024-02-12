using System.Reflection;
using Newtonsoft.Json.Linq;
using CSharpSyntax;
using FlowGraph.Logger;
using FlowGraph.Process;

namespace FlowGraph.Nodes;

public abstract class SequenceNode
{
    protected readonly List<NodeSlot> NodeSlots = new();

    public int Id { get; private set; }

    protected SequenceNode()
    {
        Id = ++_freeId;
        InitializeSlots();
    }

    public abstract SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration);

    public void ActivateOutputLink(ProcessingContext context, int id)
    {
        GetSlotById(id).RegisterNodes(context);
    }

    public NodeSlot? GetSlotById(int id)
    {
        return NodeSlots.FirstOrDefault(slot => slot.Id == id);
    }

    public object? GetValueFromSlot(int id)
    {
        var slot = GetSlotById(id);

        if (slot.ConnectedNodes.Count > 0)
        {
            var dstSlot = slot.ConnectedNodes[0];
            var node = slot.ConnectedNodes[0].Node;

            // Connected directly to a NodeSlot value (VarOut) ?
            if (dstSlot is NodeSlotVar var)
            {
                return var.Value;
            }

            if (node is VariableNode variableNode)
            {
                return variableNode.Value;
            }

            throw new InvalidOperationException($"Node({Id}) GetValueFromSlot({id}) : type of link not supported");
        }

        // if no node is connected, we take the nested value of the slot
        if (slot is NodeSlotVar slotVar)
        {
            return slotVar.Value;
        }

        return null;
    }

    public void SetValueInSlot(int id, object? value)
    {
        var slot = GetSlotById(id);

        if (slot.ConnectedNodes.Count > 0)
        {
            foreach (var other in slot.ConnectedNodes)
            {
                if (other.Node is VariableNode node)
                {
                    node.Value = value;
                }
            }
        }
        else if (slot is NodeSlotVar var)
        {
            var.Value = value;
        }
    }

    protected virtual void Load(JObject node)
    {
        Id = node["id"].Value<int>();
        if (_freeId <= Id) _freeId = Id + 1;

        X = node["x"].Value<float>();
        Y = node["y"].Value<float>();
        ZIndex = node["z"].Value<int>();

        Comment = node["comment"].Value<string>();

        foreach (var slot in NodeSlots)
        {
            var slotNode = GetSlotNode(node, slot.Id);
            if (slotNode != null)
            {
                slot.Load(slotNode);
            }
        }
    }

    private JObject GetSlotNode(JObject node, int slotId)
    {
        foreach (var slotNode in node["slots"])
        {
            if (slotNode["id"].Value<int>() == slotId)
            {
                return (JObject)slotNode;
            }
        }

        return null;
    }

    // Call after Load() to connect nodes each others
    internal virtual void ResolveLinks(JObject node, SequenceBase sequence)
    {
        foreach (var connectionNode in node.SelectTokens($"connections[?(@.source_node_id=={Id})]"))
        {
            var outputSlotId = connectionNode["source_node_slot_id"].Value<int>();
            var destNodeId = connectionNode["destination_node_id"].Value<int>();
            var destNodeInputId = connectionNode["destination_node_slot_id"].Value<int>();
            var destNode = sequence.GetNodeById(destNodeId);
            GetSlotById(outputSlotId).ConnectTo(destNode.GetSlotById(destNodeInputId));
        }
    }

    public static IList<Assembly> GetAssemblies()
    {
        var returnAssemblies = new List<Assembly>();
        var loadedAssemblies = new HashSet<string>();

        foreach (var reference in Assembly.GetEntryAssembly()!.GetReferencedAssemblies()
                     .Where(x => x.Name != null && !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System")))
        {
            if (!loadedAssemblies.Contains(reference.FullName))
            {
                var assembly = Assembly.Load(reference);
                loadedAssemblies.Add(reference.FullName);
                returnAssemblies.Add(assembly);
            }
        }

        return returnAssemblies;
    }

    public static SequenceNode? CreateNodeFromJson(JObject node)
    {
        var typeVal = node["type"].Value<string>();

        try
        {
            var type = NodeRegister.NodeTypes.FirstOrDefault(t =>
                t.AssemblyQualifiedName!
                    .Substring(0, t.AssemblyQualifiedName
                        .IndexOf(',', t.AssemblyQualifiedName
                            .IndexOf(',') + 1))
                    .Equals(typeVal));

            var sequenceNode = (SequenceNode?)Activator.CreateInstance(type);
            sequenceNode.Load(node);
            return sequenceNode;
        }
        catch (Exception ex)
        {
            LogManager.Instance.WriteException(ex);
            throw;
        }
    }

    public void RemoveAllConnections()
    {
        foreach (var slot in NodeSlots)
        {
            slot.RemoveAllConnections();
        }
    }

    protected abstract void InitializeSlots();

    public abstract SequenceNode Copy();

#if EDITOR
    private static int _freeId;

    public abstract NodeType NodeType { get; }

    public SlotAvailableFlag SlotFlag { get; protected set; }

    public abstract string Title { get; }

    public string? Comment;

    public string? CustomText;

    public double X;
    public double Y;
    public int ZIndex;

    public NodeSlot[] Slots => NodeSlots.ToArray();

    public bool IsProcessing { get; set; }

    public NodeSlot? SlotConnectorIn => NodeSlots.FirstOrDefault(slot => slot.ConnectionType == SlotType.NodeIn);

    public IEnumerable<NodeSlot> SlotConnectorOut => NodeSlots.Where(slot => slot.ConnectionType == SlotType.NodeOut);

    public int SlotConnectorOutCount => NodeSlots.Count(slot => slot.ConnectionType == SlotType.NodeOut);

    public IEnumerable<NodeSlot> SlotVariableIn => NodeSlots.Where(slot => slot.ConnectionType == SlotType.VarIn);

    public int SlotVariableInCount => NodeSlots.Count(slot => slot.ConnectionType == SlotType.VarIn);

    public IEnumerable<NodeSlot> SlotVariableOut => NodeSlots.Where(slot => slot.ConnectionType == SlotType.VarOut);

    public int SlotVariableOutCount => NodeSlots.Count(slot => slot.ConnectionType == SlotType.VarOut);

    public IEnumerable<NodeSlot> SlotVariableInOut => NodeSlots.Where(slot => slot.ConnectionType == SlotType.VarInOut);

    public int SlotVariableInOutCount => NodeSlots.Count(slot => slot.ConnectionType == SlotType.VarInOut);

    public bool HasSlotConnectorIn => (SlotFlag & SlotAvailableFlag.NodeIn) == SlotAvailableFlag.NodeIn;


    public bool HasSlotConnectorOut => (SlotFlag & SlotAvailableFlag.NodeOut) == SlotAvailableFlag.NodeOut;


    public bool HasSlotVariableIn => (SlotFlag & SlotAvailableFlag.VarIn) == SlotAvailableFlag.VarIn;


    public bool HasSlotVariableOut => (SlotFlag & SlotAvailableFlag.VarOut) == SlotAvailableFlag.VarOut;

    public void Reset()
    {
        CustomText = null;
        IsProcessing = false;
    }

    protected void AddFunctionSlot(int slotId, SlotType connectionType, SequenceFunctionSlot slot)
    {
        AddSlot(new NodeFunctionSlot(slotId, this, connectionType, slot));
    }

    protected void AddSlot(int slotId, string? text, SlotType connectionType, Type type = null,
        bool saveInternalValue = true, VariableControlType controlType = VariableControlType.ReadOnly,
        object? tag = null)
    {
        AddSlot(connectionType is SlotType.VarIn or SlotType.VarOut
            ? new NodeSlotVar(slotId, this, text, connectionType, type, controlType, tag, saveInternalValue)
            : new NodeSlot(slotId, this, text, connectionType, type, controlType, tag));
    }

    private void AddSlot(NodeSlot ite)
    {
        if (NodeSlots.Any(slot => slot.Id == ite.Id))
        {
            throw new InvalidOperationException("A slot with the Id '" + ite.Id + "' already exists.");
        }

        if (HasSlotConnectorIn == false
            && ite.ConnectionType == SlotType.NodeIn)
        {
            throw new InvalidOperationException("This type of node can not have IN connector.");
        }

        if (HasSlotConnectorOut == false
            && ite.ConnectionType == SlotType.NodeOut)
        {
            throw new InvalidOperationException("This type of node can not have OUT connector.");
        }

        if (HasSlotVariableIn == false
            && ite.ConnectionType == SlotType.VarIn)
        {
            throw new InvalidOperationException("This type of node can not have IN variable.");
        }

        if (HasSlotVariableOut == false
            && ite.ConnectionType == SlotType.VarOut)
        {
            throw new InvalidOperationException("This type of node can not have OUT variable.");
        }

        NodeSlots.Add(ite);
    }

    public void RemoveSlotById(int id)
    {
        foreach (var s in NodeSlots.Where(s => s.Id == id))
        {
            NodeSlots.Remove(s);
            break;
        }
    }

    public bool IsConnectorIn(int index)
    {
        return index < (SlotConnectorIn == null ? 0 : 1);
    }

    public virtual void Save(JObject node)
    {
        node["comment"] = Comment;
        node["id"] = Id;
        node["x"] = X;
        node["y"] = Y;
        node["z"] = ZIndex;

        var typeName = GetType().AssemblyQualifiedName!;
        var index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
        typeName = typeName.Substring(0, index);
        node["type"] = typeName;

        //Save slots
        var jsonArraySlots = new JArray();
        foreach (var slot in NodeSlots)
        {
            var nodeSlot = new JObject();
            slot.Save(nodeSlot);
            jsonArraySlots.Add(nodeSlot);
        }

        node["slots"] = jsonArraySlots;
    }

    public void SaveConnections(JObject parentNode)
    {
        var jsonArrayConnections = new JArray();

        foreach (var slot in NodeSlots)
        {
            foreach (var otherSlot in slot.ConnectedNodes)
            {
                var linkNode = new JObject();
                linkNode["source_node_id"] = Id;
                linkNode["source_node_slot_id"] = slot.Id;
                linkNode["destination_node_id"] = otherSlot.Node.Id;
                linkNode["destination_node_slot_id"] = otherSlot.Id;
                jsonArrayConnections.Add(linkNode);
            }
        }

        parentNode["connections"] = jsonArrayConnections;
    }

#endif
}