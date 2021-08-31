using System;
using System.Collections.ObjectModel;
using System.Xml;
using FlowGraphBase;
using FlowGraphBase.Logger;
using FlowSimulator.UI;

namespace FlowSimulator.FlowGraphs
{
    /// <summary>
    /// 
    /// </summary>
    public class FlowGraphManager
    {
        /// <summary>
        /// Gets
        /// </summary>
        public static FlowGraphManager Instance { get; } = new FlowGraphManager();

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<FlowGraphControlViewModel> FlowGraphList { get; } = new ObservableCollection<FlowGraphControlViewModel>();

        /// <summary>
        /// 
        /// </summary>
        private FlowGraphManager()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FlowGraphControlViewModel GetViewModelById(int id)
        {
            foreach (FlowGraphControlViewModel vm in FlowGraphList)
            {
                if (vm.ID == id)
                {
                    return vm;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Clear()
        {
            FlowGraphList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Add(FlowGraphControlViewModel viewModel)
        {
            FlowGraphList.Add(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Remove(FlowGraphControlViewModel viewModel)
        {
            FlowGraphList.Remove(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsChanges()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void Load(XmlNode node)
        {
            try
            {
                GraphDataManager.Instance.Load(node);

                int version = int.Parse(node.SelectSingleNode("GraphList").Attributes["version"].Value);

                foreach (XmlNode graphNode in node.SelectNodes("GraphList/Graph"))
                {
                    FlowGraphList.Add(new FlowGraphControlViewModel(graphNode));
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void Save(XmlNode node)
        {
            // Create all sequence to reflect all changes make (because it is not done in real time)
            foreach (FlowGraphControlViewModel wm in FlowGraphList)
            {
                wm.CreateSequence();
            }

            GraphDataManager.Instance.Save(node);

            const int version = 1;

            XmlNode list = node.SelectSingleNode("GraphList");

            list.AddAttribute("designerVersion", version.ToString());

            foreach (FlowGraphControlViewModel wm in FlowGraphList)
            {
                wm.Save(list);
            }
        }
    }
}
