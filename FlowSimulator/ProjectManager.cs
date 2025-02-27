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

        public static bool OpenFile(string fileName)
        {
            Clear();

            try
            {
                JObject jsonDocument = JObject.Parse(File.ReadAllText(fileName));

                NamedVariableManager.Instance.Load(jsonDocument);

                LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully loaded", fileName);
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
                return false;
            }

            return true;
        }

        public static bool SaveFile(string fileName)
        {
            const int version = 1;

            try
            {
                JObject rootObject = new();
                NamedVariableManager.Instance.Save(rootObject);

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
