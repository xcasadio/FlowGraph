using System.Xml;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions.FlowControl;

[Category("Flow Control"), Name("Flip Flop")]
public class FlipFlopNode :
    ActionNode
{
    public enum NodeSlotId
    {
        InEnter,
        OutA,
        OutB,
        VarOutIsA,
    }

    public override string? Title => "Flip Flop";

    public FlipFlopNode(XmlNode node)
        : base(node) { }

    public FlipFlopNode()
    { }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.InEnter, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.OutA, "A", SlotType.NodeOut);
        AddSlot((int)NodeSlotId.OutB, "B", SlotType.NodeOut);
        AddSlot((int)NodeSlotId.VarOutIsA, "IsA", SlotType.VarOut, typeof(bool));
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
            memoryItem = context.CurrentFrame.Allocate(Id, true);
        }

        if (slot.Id == (int)NodeSlotId.InEnter)
        {
            var val = (bool)memoryItem.Value;
            memoryItem.Value = !(bool)memoryItem.Value;

            SetValueInSlot((int)NodeSlotId.VarOutIsA, val);

            if (val)
            {
                ActivateOutputLink(context, (int)NodeSlotId.OutA);
            }
            else
            {
                ActivateOutputLink(context, (int)NodeSlotId.OutB);
            }
        }

        return info;
    }

    protected override SequenceNode CopyImpl()
    {
        return new FlipFlopNode();
    }
}