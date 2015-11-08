using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowSimulator.Logger;
using FlowGraphBase.Logger;
using FlowSimulator.FlowGraphs;
using FlowGraphBase;
using FlowGraphBase.Script;

namespace FlowSimulator
{
    /// <summary>
    /// 
    /// </summary>
    static internal class ProjectManager
    {
        /// <summary>
        /// 
        /// </summary>
        static public void Clear()
        {
            NamedVariableManager.Instance.Clear();
            GraphDataManager.Instance.Clear();
            FlowGraphManager.Instance.Clear();
            MainWindow.Instance.DetailsControl.DataContext = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName_"></param>
        static public bool OpenFile(string fileName_)
        {
            Clear();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName_);
                XmlNode rootNode = xmlDoc.SelectSingleNode("FlowSimulator");
                int version = int.Parse(rootNode.Attributes["version"].Value);

                NamedVariableManager.Instance.Load(rootNode);
                FlowGraphManager.Instance.Load(rootNode); // GraphDataManager.Instance.Load(rootNode) done inside

                LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully loaded", fileName_);
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
        /// <param name="fileName_"></param>
        static public bool SaveFile(string fileName_)
        {
            const int version = 1;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.AddRootNode("FlowSimulator");
                rootNode.AddAttribute("version", version.ToString());

                FlowGraphManager.Instance.Save(rootNode);
                NamedVariableManager.Instance.Save(rootNode);

                xmlDoc.Save(fileName_);

                LogManager.Instance.WriteLine(LogVerbosity.Info, "'{0}' successfully saved", fileName_);
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
