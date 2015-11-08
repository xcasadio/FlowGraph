using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    public abstract
#if EDITOR
    partial
#endif
    class EventNode
        : SequenceNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventNode(XmlNode node_)
            : base(node_)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            SlotFlag = SlotAvailableFlag.DefaultFlagEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="index_"></param>
        public void Triggered(ProcessingContext context_, int index_, object param_)
        {
            TriggeredImpl(param_);
            ActivateOutputLink(context_, index_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param_"></param>
        protected abstract void TriggeredImpl(object param_);
    }
}
