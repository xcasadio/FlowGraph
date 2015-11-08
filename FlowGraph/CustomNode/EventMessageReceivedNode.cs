using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase;
using System.Xml;
using FlowGraphBase.Node;

namespace CustomNode
{
    public class EventMessageReceivedNode
        : EventNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventMessageReceivedNode(XmlNode node_)
            : base(node_)
        {

        }

#if EDITOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventMessageReceivedNode()
            : base("Message Received Event")
        {
            AddSlot(0, "received", SlotType.NodeOut);
            AddSlot(1, "message", SlotType.VarOut);
            //TODO
            //AddSlot(1, "message", SlotType.VarOut, typeof(MessageDatas));

            //AddSlot(new NodeItemType("Message", SequenceNode.ConnectionType.Variable, typeof(string)));
        }

#endif

        /*public override void Triggered(object instigator_, int index_)
        {
            ActivateOutputLink(index_);
        }*/

        /*protected override void Load(XmlNode node_)
        {
            base.Load();
        }*/
    }
}
