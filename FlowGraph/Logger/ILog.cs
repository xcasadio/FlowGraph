namespace FlowGraph.Logger
{
    public interface ILog
    {
        void Close();

        void Write(LogVerbosity verbose, string msg);
    }
}
