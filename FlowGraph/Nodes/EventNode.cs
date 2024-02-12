using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes;

[Category("Event")]
public abstract class EventNode : SequenceNode
{
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

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        var statements = new List<StatementSyntax>();

        foreach (var slot in SlotConnectorOut)
        {
            foreach (var node in slot.ConnectedNodes)
            {
                statements.Add((StatementSyntax)node.Node.GenerateAst(classDeclaration));
            }
        }

        var parameters = new List<ParameterSyntax>();

        foreach (var slot in SlotVariableOut)
        {
            parameters.Add(Syntax.Parameter(type: slot.VariableType.Name, identifier: slot.Text.Replace(" ", string.Empty)));
        }

        return Syntax.MethodDeclaration(
            identifier: Title.Replace(" ", string.Empty),
            returnType: Syntax.ParseName("void"),
            modifiers: Modifiers.Public,
            parameterList: Syntax.ParameterList(),
            body: Syntax.Block(statements));
    }

#endif
}