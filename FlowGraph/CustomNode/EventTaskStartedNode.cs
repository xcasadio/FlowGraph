using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase;
using System.Xml;
using FlowGraphBase.Node;

namespace CustomNode
{
    public class EventTaskStartedNode
        : EventNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventTaskStartedNode(XmlNode node_)
            : base(node_)
        {

        }

#if EDITOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventTaskStartedNode()
            : base("Task Started Event")
        {
            AddSlot(0, "started", SlotType.NodeOut);
            AddSlot(1, "task name", SlotType.VarOut, typeof(string));

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
