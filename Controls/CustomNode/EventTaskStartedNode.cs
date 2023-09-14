using System.Xml;
using FlowGraph;
using FlowGraph.Attributes;
using FlowGraph.Nodes;

namespace CustomNode
{
    [Name("Task Started")]
    public class EventTaskStartedNode : EventNode
    {
        public EventTaskStartedNode(XmlNode node)
            : base(node)
        {

        }

#if EDITOR

        public EventTaskStartedNode()
        {
            AddSlot(0, "started", SlotType.NodeOut);
            AddSlot(1, "task name", SlotType.VarOut, typeof(string));

            //AddSlot(new NodeItemType("Message", SequenceNode.ConnectionType.Variable, typeof(string)));
        }

#endif

        public override string Title => "Task Started Event";

        protected override SequenceNode CopyImpl()
        {
            throw new NotImplementedException();
        }

        protected override void TriggeredImpl(object? para)
        {
            throw new NotImplementedException();
        }
    }
}
