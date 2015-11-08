using System;
using System.Collections.ObjectModel;
using System.Xml;
using FlowGraphBase.Logger;
using FlowSimulator.UI;
using FlowGraphBase;

namespace FlowSimulator.FlowGraphs
{
    /// <summary>
    /// 
    /// </summary>
    public class FlowGraphManager
    {
        #region Singleton

        static private FlowGraphManager m_Instance = new FlowGraphManager();

        /// <summary>
        /// Gets
        /// </summary>
        static public FlowGraphManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

        #endregion //Singleton

        #region Fields

        private ObservableCollection<FlowGraphControlViewModel> m_FlowGraphList = new ObservableCollection<FlowGraphControlViewModel>();

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<FlowGraphControlViewModel> FlowGraphList
        {
            get { return m_FlowGraphList; }
        }

		#endregion //Properties
	
		#region Constructors
		
        /// <summary>
        /// 
        /// </summary>
        private FlowGraphManager()
        {

        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public FlowGraphControlViewModel GetViewModelByID(int id_)
        {
            foreach (FlowGraphControlViewModel vm in m_FlowGraphList)
            {
                if (vm.ID == id_)
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
            m_FlowGraphList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Add(FlowGraphControlViewModel viewModel_)
        {
            m_FlowGraphList.Add(viewModel_);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Remove(FlowGraphControlViewModel viewModel_)
        {
            m_FlowGraphList.Remove(viewModel_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsChanges()
        {
            return false;
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            try
            {
                GraphDataManager.Instance.Load(node_);

                int version = int.Parse(node_.SelectSingleNode("GraphList").Attributes["version"].Value);

                foreach (XmlNode graphNode in node_.SelectNodes("GraphList/Graph"))
                {
                    m_FlowGraphList.Add(new FlowGraphControlViewModel(graphNode));
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Save(XmlNode node_)
        {
            // Create all sequence to reflect all changes make (because it is not done in real time)
            foreach (FlowGraphControlViewModel wm in m_FlowGraphList)
            {
                wm.CreateSequence();
            }

            GraphDataManager.Instance.Save(node_);

            const int version = 1;

            XmlNode list = node_.SelectSingleNode("GraphList");

            list.AddAttribute("designerVersion", version.ToString());

            foreach (FlowGraphControlViewModel wm in m_FlowGraphList)
            {
                wm.Save(list);
            }
        }

        #endregion // Persistence

        #endregion //Methods
    }
}
