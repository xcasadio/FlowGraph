using System.Xml;
using DotNetCodeGenerator.Ast;
using FlowGraph.Process;

namespace FlowGraph.Nodes;

public abstract class VariableNode : SequenceNode
{
#if EDITOR
    public override NodeType NodeType => NodeType.Variable;
    public NodeSlot VariableSlot => NodeSlots[0];
    public override string Title => "";

    public override void Save(XmlNode node)
    {
        base.Save(node);

        XmlNode valueNode = node.OwnerDocument!.CreateElement("Value");
        node.AppendChild(valueNode);
        SaveValue(valueNode);
    }

    protected abstract object LoadValue(XmlNode node);

    protected abstract void SaveValue(XmlNode node);
    public override Statement GenerateAst()
    {
        return new VariableStatement(new Token(TokenType.Var, "", Value), new Literal(Value));
    }

#endif

    public VariableNode()
    {
    }

    protected VariableNode(XmlNode node)
        : base(node)
    {
    }

    protected override void InitializeSlots()
    {
        SlotFlag = SlotAvailableFlag.DefaultFlagVariable;
    }

    public abstract object? Value
    {
        get;
        set;
    }

    public void Allocate(MemoryStack memoryStack)
    {
        // TODO : create function => object abstract CopyValue(object val_) ?????
        // do this only to copy the value
        var clone = (VariableNode)Copy();
        memoryStack.Allocate(Id, clone.Value);
    }

    public void Deallocate(MemoryStack memoryStack)
    {
        memoryStack.Deallocate(Id);
    }
}