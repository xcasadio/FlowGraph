using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions.FlowControl;

[Category("Flow Control"), Name("Gate")]
public class GateNode : ActionNode
{
    public enum NodeSlotId
    {
        InEnter,
        InOpen,
        InClose,
        InToggle,
        VarInStartClosed,
        Out
    }

    public override string? Title => "Gate";

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.InEnter, "Enter", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.InOpen, "Open", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.InClose, "Close", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.InToggle, "Toggle", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.VarInStartClosed, "Start Closed", SlotType.VarIn, typeof(bool));

        AddSlot((int)NodeSlotId.Out, "Exit", SlotType.NodeOut);
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var memoryItem = context.CurrentFrame.GetValueFromId(Id);

        if (memoryItem == null)
        {
            var a = GetValueFromSlot((int)NodeSlotId.VarInStartClosed);
            var state = a == null || (bool)a;
            memoryItem = context.CurrentFrame.Allocate(Id, state);
        }

        var val = (bool)memoryItem.Value;

        if (slot.Id == (int)NodeSlotId.InEnter)
        {
            if (val)
            {
                ActivateOutputLink(context, (int)NodeSlotId.Out);
            }
        }
        else if (slot.Id == (int)NodeSlotId.InOpen)
        {
            memoryItem.Value = true;
        }
        else if (slot.Id == (int)NodeSlotId.InClose)
        {
            memoryItem.Value = false;
        }
        else if (slot.Id == (int)NodeSlotId.InToggle)
        {
            memoryItem.Value = !val;
        }

        return info;
    }

    public override SequenceNode Copy()
    {
        return new GateNode();
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //Do it only once !!!
        //Add GateNodeImplementation in class member and initialize it
        var gateNodeImplementationName = nameof(GateNodeImplementation);

        classDeclaration.Members.Add(Syntax.FieldDeclaration(
            modifiers: Modifiers.Private,
            declaration: Syntax.VariableDeclaration(
                Syntax.ParseName(gateNodeImplementationName),
                new[] {
                    Syntax.VariableDeclarator(
                    "_gate", // TODO save the field name created
                        initializer: Syntax.EqualsValueClause(Syntax.ObjectCreationExpression(
                            Syntax.ParseName(gateNodeImplementationName),
                            Syntax.ArgumentList(Syntax.Argument(Syntax.LiteralExpression(true)))))) // TODO get the variable name
                    }
                )));


        //if (_gate.IsOpen)
        var statementSyntax = new BlockSyntax();
        var ifStatement = new IfStatementSyntax
        {
            Condition = new MemberAccessExpressionSyntax
            {
                Expression = new LiteralExpressionSyntax { Value = "_gate" },
                Name = (SimpleNameSyntax)Syntax.ParseName("IsOpen")
            },
            Statement = statementSyntax
        };

        //call other out Nodes
        foreach (var connectedNode in GetSlotById((int)NodeSlotId.Out).ConnectedNodes)
        {
            statementSyntax.Statements.Add((StatementSyntax)connectedNode.Node.GenerateAst(classDeclaration));
        }

        return ifStatement;
    }
}

public class GateNodeImplementation
{
    public bool IsOpen { get; private set; }

    public GateNodeImplementation(bool startClosed = false)
    {
        IsOpen = !startClosed;
    }

    public void Open()
    {
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
    }

    public void Toggle()
    {
        IsOpen = !IsOpen;
    }
}