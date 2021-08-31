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
            m_value = value;
        }

        private T m_value;

        /// <summary>
        /// 
        /// </summary>
        public T Value => m_value;
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
            m_value = value;
            m_value2 = value2;
        }

        private T m_value;
        private U m_value2;

        /// <summary>
        /// 
        /// </summary>
        public T Value => m_value;

        /// <summary>
        /// 
        /// </summary>
        public U Value2 => m_value2;
    }
}
