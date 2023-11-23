using System.Reflection;
using System.Xml;
using DotNetCodeGenerator.Ast;
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

    protected SequenceNode(XmlNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        InitializeSlots();
        Load(node);
    }

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

    protected virtual void Load(XmlNode node)
    {
        Id = int.Parse(node.Attributes["id"].Value);
        if (_freeId <= Id) _freeId = Id + 1;
        Comment = node.Attributes["comment"].Value; // EDITOR

        foreach (var slot in NodeSlots)
        {
            var nodeSlot = node.SelectSingleNode("Slot[@index='" + slot.Id + "']");
            if (nodeSlot != null)
            {
                slot.Load(nodeSlot);
            }
        }
    }

    // Call after Load() to connect nodes each others
    internal virtual void ResolveLinks(XmlNode connectionListNode, SequenceBase sequence)
    {
        foreach (XmlNode connNode in connectionListNode.SelectNodes("Connection[@srcNodeID='" + Id + "']"))
        {
            var outputSlotId = int.Parse(connNode.Attributes["srcNodeSlotID"].Value);
            var destNodeId = int.Parse(connNode.Attributes["destNodeID"].Value);
            var destNodeInputId = int.Parse(connNode.Attributes["destNodeSlotID"].Value);

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

    public static SequenceNode? CreateNodeFromXml(XmlNode node)
    {
        var typeVal = node.Attributes["type"].Value;

        try
        {
            var type = GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Single(t => t.IsClass
                             && t.IsGenericType == false
                             && t.IsInterface == false
                             && t.IsAbstract == false
                             && t.IsSubclassOf(typeof(SequenceNode))
                             && t.AssemblyQualifiedName!
                                 .Substring(0, t.AssemblyQualifiedName
                                     .IndexOf(',', t.AssemblyQualifiedName
                                         .IndexOf(',') + 1))
                                 .Equals(typeVal));

            return (SequenceNode?)Activator.CreateInstance(type, node);
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

    public virtual void Save(XmlNode seqNodeNode)
    {
        const int version = 1;
        seqNodeNode.AddAttribute("version", version.ToString());

        seqNodeNode.AddAttribute("comment", Comment);
        seqNodeNode.AddAttribute("id", Id.ToString());

        var typeName = GetType().AssemblyQualifiedName!;
        var index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
        typeName = typeName.Substring(0, index);
        seqNodeNode.AddAttribute("type", typeName);

        //Save slots
        foreach (var slot in NodeSlots)
        {
            XmlNode nodeSlot = seqNodeNode.OwnerDocument.CreateElement("Slot");
            seqNodeNode.AppendChild(nodeSlot);
            slot.Save(nodeSlot);
        }
    }

    public void SaveConnections(XmlNode connectionListNode)
    {
        const int versionConnection = 1;
        foreach (var slot in NodeSlots)
        {
            foreach (var otherSlot in slot.ConnectedNodes)
            {
                XmlNode linkNode = connectionListNode.OwnerDocument.CreateElement("Connection");
                connectionListNode.AppendChild(linkNode);

                linkNode.AddAttribute("version", versionConnection.ToString());

                linkNode.AddAttribute("srcNodeID", Id.ToString());
                linkNode.AddAttribute("srcNodeSlotID", slot.Id.ToString());
                linkNode.AddAttribute("destNodeID", otherSlot.Node.Id.ToString());
                linkNode.AddAttribute("destNodeSlotID", otherSlot.Id.ToString());
            }
        }
    }

    public abstract Statement GenerateAst();

#endif

}