using System.Xml;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Process;

namespace FlowGraph.Nodes.StandardAction;

[Category("Time"), Name("Delay")]
public class DelayNode
    : ActionNode
{
    public enum NodeSlotId
    {
        In,
        Out,
        Delay
    }

    //TimeSpan _StartTime = TimeSpan.Zero;

    public override string Title => "Delay";

    public DelayNode(XmlNode node)
        : base(node)
    {

    }

    public DelayNode()
    {

    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);

        AddSlot((int)NodeSlotId.Delay, "Duration (ms)", SlotType.VarIn, typeof(int));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var intVal = GetValueFromSlot((int)NodeSlotId.Delay);

        if (intVal == null)
        {
            info.State = LogicState.Ok;
            info.ErrorMessage = "Please connect a integer variable node";

            LogManager.Instance.WriteLine(LogVerbosity.Error,
                "{0} : {1}.",
                Title, info.ErrorMessage);

            return info;
        }

        var memoryItem = context.CurrentFrame.GetValueFromId(Id);

        var startTime = (TimeSpan)memoryItem.Value!;

        var delay = (int)intVal;
        var delayDouble = delay / 1000.0;
        var t = DateTime.Now.TimeOfDay.Subtract(startTime);
        var totalSecs = t.TotalSeconds;

        //this is the first time, so we set the current time
        if (startTime == TimeSpan.Zero)
        {
            totalSecs = 0.0;
            startTime = DateTime.Now.TimeOfDay;
            memoryItem.Value = startTime;
        }
        else if (totalSecs >= delayDouble)
        {
            startTime = TimeSpan.Zero;
            memoryItem.Value = startTime;
            ActivateOutputLink(context, (int)NodeSlotId.Out);
            CustomText = String.Empty;
            return info;
        }

        CustomText = $"Delay ({delayDouble - totalSecs:0.000} seconds left)";

        var nodeSlot = GetSlotById((int)NodeSlotId.In);
        if (nodeSlot != null)
        {
            context.RegisterNextExecution(nodeSlot);
        }
        else
        {
            Logger.LogManager.Instance.WriteLine(LogVerbosity.Error, $"Can't find the slot {(int)NodeSlotId.In}");
        }

        return info;
    }

    protected override SequenceNode CopyImpl()
    {
        return new DelayNode();
    }
}