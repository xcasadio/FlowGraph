namespace FlowGraph
{

    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    public class EventArgs<T, TU> : EventArgs
    {
        public EventArgs(T value, TU value2)
        {
            Value = value;
            Value2 = value2;
        }

        public T Value { get; }

        public TU Value2 { get; }
    }
}
