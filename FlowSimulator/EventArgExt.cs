using System;

namespace FlowSimulator
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArg1Param<T> : EventArgs
    {
        /// <summary>
        /// Gets
        /// </summary>
        public T Arg
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public EventArg1Param(T arg)
        {
            Arg = arg;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArg2Params<T, U> : EventArg1Param<T>
    {
        /// <summary>
        /// Gets
        /// </summary>
        public U Arg2
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public EventArg2Params(T arg1, U arg2)
            : base(arg1)
        {
            Arg2 = arg2;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventArg3Params<T, U, V> : EventArg2Params<T, U>
    {
        /// <summary>
        /// Gets
        /// </summary>
        public V Arg3
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public EventArg3Params(T arg1, U arg2, V arg3)
            : base(arg1, arg2)
        {
            Arg3 = arg3;
        }
    }
}
