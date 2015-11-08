using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase;
using System.Xml;
using FlowGraphBase.Node;

namespace CustomNode
{
    public 
#if EDITOR
    partial
#endif
    class EventTestStartedNode
        : EventNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventTestStartedNode(XmlNode node_)
            : base(node_)
        {

        }

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
