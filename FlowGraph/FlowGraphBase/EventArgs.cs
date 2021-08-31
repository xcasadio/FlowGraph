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
    public class EventArgs<T, U> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        public EventArgs(T value, U value2)
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
        public U Value2 { get; }
    }
}
