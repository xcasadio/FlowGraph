using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase.Logger;

namespace FlowSimulator.Logger
{
    /// <summary>
    /// 
    /// </summary>
    public class LogCEvent : ILog
    {
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verbose"></param>
        /// <param name="args_"></param>
        public void Write(LogVerbosity verbose, string msg_)
        {
            msg_ = msg_.Replace("{", "{{").Replace("}", "}}");

            switch (verbose)
            {
                case LogVerbosity.Trace:
                    LogFile.OnDebug(msg_);
                    break;

                case LogVerbosity.Debug:
                    LogFile.OnDebug(msg_);
                    break;

                case LogVerbosity.Info:
                    LogFile.OnInfo(msg_);
                    break;

                case LogVerbosity.Warning:
                    LogFile.OnWarning(msg_);
                    break;

                case LogVerbosity.Error:
                    LogFile.OnError(msg_);
                    break;
            }
        }
    }
}
