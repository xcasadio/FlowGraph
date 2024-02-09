using System.Xml;
using CSharpSyntax;
using FlowGraph.Process;

namespace FlowGraph.Nodes;

public abstract class VariableNode : SequenceNode
{
    public abstract object? Value { get; set; }

    protected VariableNode()
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

    public override ExpressionSyntax GenerateAst()
    {
        throw new NotImplementedException();
        /*
        return Syntax.LocalDeclarationStatement(
                    Syntax.VariableDeclaration(
                        Syntax.ParseName(Value == null ? nameof(Object) : Value.GetType().Name),
                        new[]
                        {
                            Syntax.VariableDeclarator(
                                "variableNodeValue",
                                initializer: Syntax.EqualsValueClause(Syntax.LiteralExpression(Value))
                            )
                        }));*/
    }

#endif
}