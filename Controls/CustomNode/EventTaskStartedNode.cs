using System.Xml;
using FlowGraph;
using FlowGraph.Attributes;
using FlowGraph.Nodes;

namespace CustomNode
{
    [Name("Task Started")]
    public class EventTaskStartedNode : EventNode
    {
        public override string Title => "Task Started Event";

        public override SequenceNode Copy()
        {
            throw new NotImplementedException();
        }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot(0, "started", SlotType.NodeOut);
            AddSlot(1, "task name", SlotType.VarOut, typeof(string));
        }

        protected override void TriggeredImpl(object? para)
        {
            throw new NotImplementedException();
        }
    }
}
