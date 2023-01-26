using System.Xml;
using FlowGraph.Process;

namespace FlowGraph.Node
{
    public abstract class EventNode : SequenceNode
    {
#if EDITOR
        public override NodeType NodeType => NodeType.Event;

        protected EventNode()
        {
        }
#endif

        protected EventNode(XmlNode node)
            : base(node)
        {
        }

        protected override void InitializeSlots()
        {
            SlotFlag = SlotAvailableFlag.DefaultFlagEvent;
        }

        public void Triggered(ProcessingContext context, int index, object? para)
        {
            TriggeredImpl(para);
            ActivateOutputLink(context, index);
        }

        protected abstract void TriggeredImpl(object? para);
    }
}
