using System.IO;
using FlowGraph;
using FlowGraph.Logger;
using Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlowSimulator
{
    internal static class ProjectManager
    {
        public static void Clear()
        {
            NamedVariableManager.Instance.Clear();
            MainWindow.Instance.DetailsControl.DataContext = null;
        }

        public static bool OpenFile(string fileName, FlowGraphManager flowGraphManager)
        {
            Clear();

            try
            {
                JObject rootObject = JObject.Parse(File.ReadAllText(fileName));

                NamedVariableManager.Instance.Load(rootObject);
                flowGraphManager.Load(rootObject);

                LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully loaded", fileName);
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
                return false;
            }

            return true;
        }

        public static bool SaveFile(string fileName, FlowGraphManager flowGraphManager)
        {
            const int version = 1;

            try
            {
                JObject rootObject = new();
                NamedVariableManager.Instance.Save(rootObject);
                flowGraphManager.Save(rootObject);

                using StreamWriter file = File.CreateText(fileName);
                using JsonTextWriter writer = new JsonTextWriter(file) { Formatting = Formatting.Indented };
                rootObject.WriteTo(writer);

                LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully saved", fileName);
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
                return false;
            }

            return true;
        }
    }
}
