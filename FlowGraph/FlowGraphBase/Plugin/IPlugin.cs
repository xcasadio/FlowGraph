using System.Collections.Generic;

namespace FlowGraphBase.Plugin
{
    public interface IPlugin
    {
        IEnumerable<string> GetNodeNames();
        IEnumerable<object> GetEditor();

        void Load();
        void UnLoad();
    }
}
