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
        public override NodeType NodeType => NodeType.Event;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public EventNode()
        {
        }
    }
}
