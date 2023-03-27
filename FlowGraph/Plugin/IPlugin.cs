namespace FlowGraph.Plugin
{
    public interface IPlugin
    {
        IEnumerable<string> GetNodeNames();
        IEnumerable<object> GetEditor();

        void Load();
        void UnLoad();
    }
}
