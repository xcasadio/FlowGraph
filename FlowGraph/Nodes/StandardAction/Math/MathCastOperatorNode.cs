using System.Xml;
using FlowGraph.Logger;
using FlowGraph.Process;

namespace FlowGraph.Nodes.StandardAction.Math;

public abstract class MathCastOperatorNode<TIn, TOut> : ActionNode
{
    public enum NodeSlotId
    {
        In,
        Out,
        VarA,
        VarResult
    }

    public MathCastOperatorNode(XmlNode node)
        : base(node)
    {

    }

    public MathCastOperatorNode()
    {

    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

        AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(TIn), false);
        AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(TOut), false);
    }

    public abstract TOut? DoActivateLogic(TIn a);

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var objA = GetValueFromSlot((int)NodeSlotId.VarA);

        if (objA == null)
        {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a variable node into the slot A";

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : {1}.",
                Title, info.ErrorMessage);
        }

        if (objA != null)
        {
            SetValueInSlot((int)NodeSlotId.VarResult, DoActivateLogic((TIn)objA));
        }

        ActivateOutputLink(context, (int)NodeSlotId.Out);

        return info;
    }
}