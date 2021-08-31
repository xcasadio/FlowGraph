using System;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public EventArgs(T value)
        {
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Value { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class EventArgs<T, TU> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        public EventArgs(T value, TU value2)
        {
            Value = value;
            Value2 = value2;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// 
        /// </summary>
        public TU Value2 { get; }
    }
}
