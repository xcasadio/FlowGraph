using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes;

[Category("Event")]
public abstract class EventNode : SequenceNode
{
    protected EventNode(XmlNode node)
        : base(node)
    {
    }

    protected override void InitializeSlots()
    {
        SlotFlag = SlotAvailableFlag.DefaultFlagEvent;
    }

    public void Triggered(ProcessingContext context, int index, object? para)
    {
        TriggeredImpl(para);
        ActivateOutputLink(context, index);
    }

    protected abstract void TriggeredImpl(object? para);

#if EDITOR
    public override NodeType NodeType => NodeType.Event;

    protected EventNode()
    {
    }

    public override MemberDeclarationSyntax GenerateAst()
    {
        var statements = new List<StatementSyntax>();

        foreach (var slot in SlotConnectorOut)
        {
            foreach (var node in slot.ConnectedNodes)
            {
                statements.Add((StatementSyntax)node.Node.GenerateAst());
            }
        }

        var parameters = new List<ParameterSyntax>();

        foreach (var slot in SlotVariableOut)
        {
            parameters.Add(Syntax.Parameter(type: slot.VariableType.Name, identifier: slot.Text.Replace(" ", "_")));
        }

        return Syntax.MethodDeclaration(
            identifier: Title,
            returnType: Syntax.ParseName("void"),
            modifiers: Modifiers.Public,
            parameterList: Syntax.ParameterList(),
            body: Syntax.Block(statements));
    }

#endif
}