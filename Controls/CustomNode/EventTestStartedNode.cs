using System.Xml;
using FlowGraph;
using FlowGraph.Node;

namespace CustomNode
{
    [Name("Test Started")]
    public class EventTestStartedNode : EventNode
    {
#if EDITOR
        public EventTestStartedNode()
        {
            AddSlot(0, "Test started", SlotType.NodeOut);

            //AddSlot(new NodeItemType("Message", SequenceNode.ConnectionType.Variable, typeof(string)));
        }

        protected override SequenceNode CopyImpl()
        {
            return new EventTestStartedNode();
        }
#endif

        public override string Title => "Test Started Event";

        public EventTestStartedNode(XmlNode node)
            : base(node)
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
    }
}
