using System.Xml;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions.FlowControl;

[Category("Flow Control"), Name("For Loop")]
public class ForLoopNode :
    ActionNode
{
    struct ForLoopNodeInfo
    {
        public int Counter;
        public bool IsWaitingLoopBody;
    }

    public enum NodeSlotId
    {
        In,
        OutLoop,
        OutCompleted,
        VarInFirstIndex,
        VarInLastIndex,
        VarOutIndex,
    }

    public override string? Title => "For Loop";

    public ForLoopNode(XmlNode node) : base(node) { }

    public ForLoopNode() { }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.VarInFirstIndex, "First Index", SlotType.VarIn, typeof(int));
        AddSlot((int)NodeSlotId.VarInLastIndex, "Last Index", SlotType.VarIn, typeof(int));

        AddSlot((int)NodeSlotId.OutLoop, "Loop Body", SlotType.NodeOut);
        AddSlot((int)NodeSlotId.VarOutIndex, "Index", SlotType.VarOut, typeof(int));
        AddSlot((int)NodeSlotId.OutCompleted, "Completed", SlotType.NodeOut);
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        if (slot.Id == (int)NodeSlotId.In)
        {
            int firstIndex = 0, lastIndex = -1;

            var objFirstIndex = GetValueFromSlot((int)NodeSlotId.VarInFirstIndex);

            if (objFirstIndex == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot First Index";
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : For Loop failed. {1}.",
                    Title, info.ErrorMessage);
                return info;
            }

            if (objFirstIndex != null)
            {
                firstIndex = (int)objFirstIndex;
            }

            var objLastIndex = GetValueFromSlot((int)NodeSlotId.VarInLastIndex);

            if (objLastIndex == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot Last Index";
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : For Loop failed. {1}.",
                    Title, info.ErrorMessage);
                return info;
            }

            if (objLastIndex != null)
            {
                lastIndex = (int)objLastIndex;
            }

            var memoryItem = context.CurrentFrame.GetValueFromId(Id);

            if (memoryItem == null)
            {
                memoryItem = context.CurrentFrame.Allocate(Id, new ForLoopNodeInfo { Counter = firstIndex, IsWaitingLoopBody = false });
            }

            var memoryInfo = (ForLoopNodeInfo)memoryItem.Value;

            if (memoryInfo.IsWaitingLoopBody == false)
            {
                SetValueInSlot((int)NodeSlotId.VarOutIndex, memoryInfo.Counter);

                if (memoryInfo.Counter <= lastIndex)
                {
                    memoryInfo.IsWaitingLoopBody = true;
                    memoryInfo.Counter++;
                    memoryItem.Value = memoryInfo;

                    // register again this node in order to active itself
                    // after the sequence activated by the loop body slot
                    // is finished
                    context.RegisterNextExecution(GetSlotById((int)NodeSlotId.In));
                    var newContext = context.PushNewContext();
                    newContext.Finished += OnLoopBodyFinished;
                    ActivateOutputLink(newContext, (int)NodeSlotId.OutLoop);
                }
                else
                {
                    context.CurrentFrame.Deallocate(Id);
                    ActivateOutputLink(context, (int)NodeSlotId.OutCompleted);
                }
            }
        }

        return info;
    }

    void OnLoopBodyFinished(object sender, EventArgs e)
    {
        var context = sender as ProcessingContext;
        context.Finished -= OnLoopBodyFinished;

        var memoryItem = context.Parent.CurrentFrame.GetValueFromId(Id);
        var memoryInfo = (ForLoopNodeInfo)memoryItem.Value;
        memoryInfo.IsWaitingLoopBody = false;
        memoryItem.Value = memoryInfo;
    }

    protected override SequenceNode CopyImpl()
    {
        return new ForLoopNode();
    }
}