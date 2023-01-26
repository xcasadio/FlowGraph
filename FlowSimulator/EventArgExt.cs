using System;

namespace FlowSimulator
{
    public class EventArg1Param<T> : EventArgs
    {
        public T Arg
        {
            get;
        }

        public EventArg1Param(T arg)
        {
            Arg = arg;
        }
    }

    public class EventArg2Params<T, TU> : EventArg1Param<T>
    {
        public TU Arg2
        {
            get;
        }

        public EventArg2Params(T arg1, TU arg2)
            : base(arg1)
        {
            Arg2 = arg2;
        }
    }

    public class EventArg3Params<T, TU, TV> : EventArg2Params<T, TU>
    {
        public TV Arg3
        {
            get;
        }

        public EventArg3Params(T arg1, TU arg2, TV arg3)
            : base(arg1, arg2)
        {
            Arg3 = arg3;
        }
    }
}
