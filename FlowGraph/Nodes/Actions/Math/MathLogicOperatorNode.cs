using FlowGraph.Logger;
using FlowGraph.Process;
using Logger;

namespace FlowGraph.Nodes.Actions.Math;

public abstract class MathLogicOperatorNode<T> : ActionNode
{
    public enum NodeSlotId
    {
        In,
        OutTrue,
        OutFalse,
        VarA,
        VarB
    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.OutTrue, "True", SlotType.NodeOut);
        AddSlot((int)NodeSlotId.OutFalse, "False", SlotType.NodeOut);

        AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(T));
        AddSlot((int)NodeSlotId.VarB, "B", SlotType.VarIn, typeof(T));
    }

    public abstract bool DoActivateLogic(T a, T b);

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
            info.State = LogicState.Error;
            info.ErrorMessage = "Please connect a variable node into the slot A";

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : {1}.",
                Title, info.ErrorMessage);
        }
        else if (objB == null)
        {
            info.State = LogicState.Error;
            info.ErrorMessage = "Please connect a variable node into the slot B";

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : {1}.",
                Title, info.ErrorMessage);
        }

        if (objA != null
            && objB != null)
        {
            var res = DoActivateLogic((T)objA, (T)objB);

            if (res) ActivateOutputLink(context, (int)NodeSlotId.OutTrue);
            else ActivateOutputLink(context, (int)NodeSlotId.OutFalse);
        }

        return info;
    }
}