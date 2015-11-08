using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class EventNode
        : SequenceNode
    {
        /// <summary>
        /// 
        /// </summary>
        public override NodeType NodeType
        {
            get { return NodeType.Event; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public EventNode()
            : base()
        {
        }
    }
}
