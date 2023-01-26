namespace FlowGraphBase.Node
{
    public abstract partial class EventNode
        : SequenceNode
    {
        public override NodeType NodeType => NodeType.Event;

        public EventNode()
        {
        }
    }
}
