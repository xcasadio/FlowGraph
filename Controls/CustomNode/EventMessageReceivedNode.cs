using System.Xml;
using FlowGraph;
using FlowGraph.Attributes;
using FlowGraph.Nodes;

namespace CustomNode
{
    [Name("Message Received")]
    public class EventMessageReceivedNode : EventNode
    {
        public EventMessageReceivedNode()
        {
            AddSlot(0, "received", SlotType.NodeOut);
            AddSlot(1, "message", SlotType.VarOut);
            //TODO
            //AddSlot(1, "message", SlotType.VarOut, typeof(MessageDatas));

            //AddSlot(new NodeItemType("Message", SequenceNode.ConnectionType.Variable, typeof(string)));
        }

        public override string Title => "Test Started Event";

        public override SequenceNode Copy()
        {
            throw new NotImplementedException();
        }

        protected override void TriggeredImpl(object? para)
        {
            throw new NotImplementedException();
        }
    }
}
