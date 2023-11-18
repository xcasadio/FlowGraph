using System.Xml;
using FlowGraph;
using FlowGraph.Attributes;
using FlowGraph.Nodes;

namespace CustomNode
{
    [Name("Test Started")]
    public class EventNodeTestStarted : EventNode
    {
        public override string Title => "Test Started Event";

        public EventNodeTestStarted(XmlNode node)
            : base(node)
        {

        }

        public EventNodeTestStarted()
        {

        }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot(0, "Started", SlotType.NodeOut);
            AddSlot(1, "Task name", SlotType.VarOut, typeof(string));
        }

        protected override void TriggeredImpl(object? para)
        {
            SetValueInSlot(1, para);
        }

        public override SequenceNode Copy()
        {
            return new EventNodeTestStarted();
        }
    }
}
