using FlowGraph.Logger;
using FlowGraph.Process;
using Logger;

namespace FlowGraph.Nodes.Actions.Math;

public abstract class MathOperatorNode<T> : ActionNode
{
    public enum NodeSlotId
    {
        In,
        Out,
        VarA,
        VarB,
        VarResult
    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

        AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(T));
        AddSlot((int)NodeSlotId.VarB, "B", SlotType.VarIn, typeof(T));
        AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(T));
    }

    public abstract T? DoActivateLogic(T a, T b);

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var objA = GetValueFromSlot((int)NodeSlotId.VarA);
        var objB = GetValueFromSlot((int)NodeSlotId.VarB);

        if (objA == null)
        {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a variable node into the slot A";
            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : Addition failed. {1}.",
                Title, info.ErrorMessage);
        }
        else if (objB == null)
        {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a variable node into the slot B";

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : Addition failed. {1}.",
                Title, info.ErrorMessage);
        }

        if (objA != null
            && objB != null)
        {
            SetValueInSlot((int)NodeSlotId.VarResult, DoActivateLogic((T)objA, (T)objB));
        }

        ActivateOutputLink(context, (int)NodeSlotId.Out);

        return info;
    }
}