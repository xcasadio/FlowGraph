using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase;
using System.Xml;
using FlowGraphBase.Node;

namespace CustomNode
{
    public partial class EventTestStartedNode
        : EventNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventTestStartedNode()
            : base("Test Started Event")
        {
            AddSlot(0, "Test started", SlotType.NodeOut);

            //AddSlot(new NodeItemType("Message", SequenceNode.ConnectionType.Variable, typeof(string)));
        }
    }
}
