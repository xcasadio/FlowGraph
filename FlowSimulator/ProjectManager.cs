using System.Xml;
using FlowGraphBase.Logger;
using FlowSimulator.FlowGraphs;
using FlowGraphBase;

namespace FlowSimulator
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ProjectManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Clear()
        {
            NamedVariableManager.Instance.Clear();
            GraphDataManager.Instance.Clear();
            FlowGraphManager.Instance.Clear();
            MainWindow.Instance.DetailsControl.DataContext = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static bool OpenFile(string fileName)
        {
            Clear();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlNode rootNode = xmlDoc.SelectSingleNode("FlowSimulator");

                if (rootNode?.Attributes.GetNamedItem("version") != null)
                {
                    int version = int.Parse(rootNode.Attributes["version"].Value);
                }

                NamedVariableManager.Instance.Load(rootNode);
                FlowGraphManager.Instance.Load(rootNode); // GraphDataManager.Instance.Load(rootNode) done inside

                LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully loaded", fileName);
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static bool SaveFile(string fileName)
        {
            const int version = 1;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.AddRootNode("FlowSimulator");
                rootNode.AddAttribute("version", version.ToString());

                FlowGraphManager.Instance.Save(rootNode);
                NamedVariableManager.Instance.Save(rootNode);

                xmlDoc.Save(fileName);

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
