using System.Xml;
using FlowGraph.Process;

namespace FlowGraph.Nodes;

public abstract class ActionNode : SequenceNode
{
    public enum LogicState
    {
        Ok,
        Warning,
        Error
    }

    public struct ProcessingInfo
    {
        public LogicState State;
        public string ErrorMessage;
    }

    public ProcessingInfo State { get; private set; }

    public string ErrorMessage { get; internal set; } = "";

    protected ActionNode()
    {
    }

    protected ActionNode(XmlNode node)
        : base(node)
    {

    }

    protected override void InitializeSlots()
    {
        SlotFlag = SlotAvailableFlag.All;
    }

    public ProcessingInfo Activate(ProcessingContext context, NodeSlot slot)
    {
        State = ActivateLogic(context, slot);
        return State;
    }

    public abstract ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot);


#if EDITOR
    public override NodeType NodeType => NodeType.Action;

#endif
}