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
        /// <param name="msg"></param>
        public void Write(LogVerbosity verbose, string msg)
        {
            msg = msg.Replace("{", "{{").Replace("}", "}}");

            switch (verbose)
            {
                case LogVerbosity.Trace:
                    LogFile.OnDebug(msg);
                    break;

                case LogVerbosity.Debug:
                    LogFile.OnDebug(msg);
                    break;

                case LogVerbosity.Info:
                    LogFile.OnInfo(msg);
                    break;

                case LogVerbosity.Warning:
                    LogFile.OnWarning(msg);
                    break;

                case LogVerbosity.Error:
                    LogFile.OnError(msg);
                    break;
            }
        }
    }
}
