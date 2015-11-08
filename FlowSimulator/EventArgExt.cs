using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg_"></param>
        public EventArg1Param(T arg_)
        {
            Arg = arg_;
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
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1_"></param>
        /// <param name="arg2_"></param>
        public EventArg2Params(T arg1_, U arg2_)
            : base(arg1_)
        {
            Arg2 = arg2_;
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
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1_"></param>
        /// <param name="arg2_"></param>
        /// <param name="arg3_"></param>
        public EventArg3Params(T arg1_, U arg2_, V arg3_)
            : base(arg1_, arg2_)
        {
            Arg3 = arg3_;
        }
    }
}
