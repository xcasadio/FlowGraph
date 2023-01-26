using System.Xml;
using FlowGraph.Node;

namespace CustomNode
{
    public class EventMessageReceivedNode : EventNode
    {
        public EventMessageReceivedNode(XmlNode node) : base(node)
        {

        }

#if EDITOR

        public EventMessageReceivedNode()
        {
            AddSlot(0, "received", SlotType.NodeOut);
            AddSlot(1, "message", SlotType.VarOut);
            //TODO
            //AddSlot(1, "message", SlotType.VarOut, typeof(MessageDatas));

            //AddSlot(new NodeItemType("Message", SequenceNode.ConnectionType.Variable, typeof(string)));
        }

#endif

        public override string Title => "Test Started Event";

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
