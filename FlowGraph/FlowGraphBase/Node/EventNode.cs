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
        public EventNode(XmlNode node)
            : base(node)
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
        public void Triggered(ProcessingContext context, int index, object para)
        {
            TriggeredImpl(para);
            ActivateOutputLink(context, index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para_"></param>
        protected abstract void TriggeredImpl(object para);
    }
}
