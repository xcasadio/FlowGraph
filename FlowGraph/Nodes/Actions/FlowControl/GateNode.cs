using System.Xml;
using DotNetCodeGenerator.Ast;
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

    public GateNode(XmlNode node) : base(node)
    { }

    public GateNode()
    { }

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

    public override Statement GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}