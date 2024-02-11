using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions.FlowControl;

[Category("Flow Control"), Name("Do Once")]
public class DoOnceNode : ActionNode
{
    public enum NodeSlotId
    {
        InEnter,
        InReset,
        Out,
    }

    public override string? Title => "Do Once";

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.InEnter, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.InReset, "Reset", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var memoryItem = context.CurrentFrame.GetValueFromId(slot.Id);

        if (memoryItem == null)
        {
            memoryItem = context.CurrentFrame.Allocate(Id, false);
        }

        if (slot.Id == (int)NodeSlotId.InReset)
        {
            memoryItem.Value = false;
        }
        else if (slot.Id == (int)NodeSlotId.InEnter)
        {
            if ((bool)memoryItem.Value == false)
            {
                memoryItem.Value = true;
                ActivateOutputLink(context, (int)NodeSlotId.Out);
            }
        }

        return info;
    }

    public override SequenceNode Copy()
    {
        return new DoOnceNode();
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}