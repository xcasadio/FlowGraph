namespace FlowGraphBase.Logger
{
    /// <summary>
    /// Interface for all Logger
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verbose"></param>
        /// <param name="msg"></param>
        void Write(LogVerbosity verbose, string msg);
    }
}
