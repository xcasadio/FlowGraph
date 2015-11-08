using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase;
using System.Xml;
using FlowGraphBase.Node;
using FlowGraphBase.Node.StandardNode;

namespace CustomNode
{
    public class SequenceNodeFactory
        : ISequenceNodeFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <returns></returns>
        public SequenceNode CreateNode(XmlNode node_)
        {
            if (node_.Attributes["type"].Value.Equals(typeof(AdditionNodeInt).FullName) == true)
            {
                return new AdditionNodeInt(node_);
            }
            else if (node_.Attributes["type"].Value.Equals(typeof(LogMessageNode).FullName) == true)
            {
                return new LogMessageNode(node_);
            }
            else if (node_.Attributes["type"].Value.Equals(typeof(EventTestStartedNode).FullName) == true)
            {
                return new EventTestStartedNode(node_);
            }
            else if (node_.Attributes["type"].Value.Equals(typeof(EventTaskStartedNode).FullName) == true)
            {
                return new EventTaskStartedNode(node_);
            }
            else if (node_.Attributes["type"].Value.Equals(typeof(EventMessageReceivedNode).FullName) == true)
            {
                return new EventMessageReceivedNode(node_);
            }
            else if (node_.Attributes["type"].Value.Equals(typeof(VariableNodeInt).FullName) == true)
            {
                return new VariableNodeInt(node_);
            }

            return null;
        }
    }
}
