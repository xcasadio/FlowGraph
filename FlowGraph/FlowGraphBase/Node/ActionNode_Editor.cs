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
    public partial class ActionNode
    {
        /// <summary>
        /// 
        /// </summary>
        public override NodeType NodeType
        {
            get { return NodeType.Action; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqMgr_"></param>
        public ActionNode()
            : base()
        {
        }
    }
}
