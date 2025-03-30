using System.Xml;
using FlowGraph;
using FlowGraph.Attributes;
using FlowGraph.Nodes;

namespace CustomNode
{
    [Name("Test Started")]
    public class EventTestStartedNode : EventNode
    {
#if EDITOR
        public override SequenceNode Copy()
        {
            return new EventTestStartedNode();
        }
#endif

        public override string Title => "Test Started Event";
        

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot(0, "started", SlotType.NodeOut);
            AddSlot(1, "test name", SlotType.VarOut, typeof(string));
        }

        protected override void TriggeredImpl(object? para)
        {
            SetValueInSlot(1, para);
        }
    }
}
