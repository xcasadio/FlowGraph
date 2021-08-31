using System.Collections.Generic;
using FlowSimulator.UI;
using NetworkModel;
using System.Windows.Media;
using System.Windows;

namespace FlowSimulator.Undo
{
    /// <summary>
    /// Used to copy info from a ConnectionViewModel
    /// </summary>
    struct ConnectionInfo
    {
        public ConnectionViewModel ConnectionVM;
        public ConnectorViewModel DestConnector;
        public Point DestConnectorHotspot;
        public PointCollection Points;
        public ConnectorViewModel SourceConnector;
        public Point SourceConnectorHotspot;
    }

    /// <summary>
    /// 
    /// </summary>
    class CreateConnectionUndoCommand : IUndoCommand
    {
		#region Fields

        FlowGraphControlViewModel m_FlowGraphVM;

        ConnectionViewModel m_ConnectionVM;
        ConnectorViewModel m_DestConnector;
        Point m_DestConnectorHotspot;
        PointCollection m_Points;
        ConnectorViewModel m_SourceConnector;
        Point m_SourceConnectorHotspot;

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public CreateConnectionUndoCommand(FlowGraphControlViewModel fgvm_, ConnectionViewModel connectionVM_)
        {
            m_FlowGraphVM = fgvm_;
            //m_ConnectionVM = connectionVM_.Copy();
            m_DestConnector = connectionVM_.DestConnector;
            m_DestConnectorHotspot = connectionVM_.DestConnectorHotspot;
            m_Points = connectionVM_.Points;
            m_SourceConnector = connectionVM_.SourceConnector;
            m_SourceConnectorHotspot = connectionVM_.SourceConnectorHotspot;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            m_FlowGraphVM.AddConnection(m_ConnectionVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            ConnectionViewModel copy = new ConnectionViewModel();
            copy.DestConnector = m_DestConnector;
            copy.DestConnectorHotspot = m_DestConnectorHotspot;
            copy.Points = m_Points;
            copy.SourceConnector = m_SourceConnector;
            copy.SourceConnectorHotspot = m_SourceConnectorHotspot;

            m_FlowGraphVM.DeleteConnection(copy);
            m_ConnectionVM = copy;

            //m_FlowGraphVM.DeleteConnection(m_ConnectionVM);
        }

		#endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel m_FlowGraphVM;

        ConnectionViewModel m_ConnectionVM;
        ConnectorViewModel m_DestConnector;
        Point m_DestConnectorHotspot;
        PointCollection m_Points;
        ConnectorViewModel m_SourceConnector;
        Point m_SourceConnectorHotspot;

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DeleteConnectionUndoCommand(FlowGraphControlViewModel fgvm_, ConnectionViewModel connectionVM_)
        {
            m_FlowGraphVM = fgvm_;
            //m_ConnectionVM = connectionVM_.Copy();
            m_DestConnector = connectionVM_.DestConnector;
            m_DestConnectorHotspot = connectionVM_.DestConnectorHotspot;
            m_Points = connectionVM_.Points;
            m_SourceConnector = connectionVM_.SourceConnector;
            m_SourceConnectorHotspot = connectionVM_.SourceConnectorHotspot;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            m_FlowGraphVM.DeleteConnection(m_ConnectionVM);
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            ConnectionViewModel copy = new ConnectionViewModel();
            copy.DestConnector = m_DestConnector;
            copy.DestConnectorHotspot = m_DestConnectorHotspot;
            copy.Points = m_Points;
            copy.SourceConnector = m_SourceConnector;
            copy.SourceConnectorHotspot = m_SourceConnectorHotspot;

            m_FlowGraphVM.AddConnection(copy);
            m_ConnectionVM = copy;
        }

		#endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class CreateConnectionsUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel m_FlowGraphVM;
        IEnumerable<ConnectionViewModel> m_ConnectionsVM;

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public CreateConnectionsUndoCommand(FlowGraphControlViewModel fgvm_, IEnumerable<ConnectionViewModel> connectionsVM_)
        {
            m_FlowGraphVM = fgvm_;
            m_ConnectionsVM = connectionsVM_;
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            m_FlowGraphVM.AddConnections(m_ConnectionsVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            m_FlowGraphVM.DeleteConnections(m_ConnectionsVM);
        }

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionsUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel m_FlowGraphVM;
        List<ConnectionInfo> m_ConnectionInfoList = new List<ConnectionInfo>();

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DeleteConnectionsUndoCommand(FlowGraphControlViewModel fgvm_, IEnumerable<ConnectionViewModel> connectionsVM_)
        {
            m_FlowGraphVM = fgvm_;

            foreach (ConnectionViewModel c in connectionsVM_)
            {
                m_ConnectionInfoList.Add(new ConnectionInfo
                    {
                        ConnectionVM = null,
                        DestConnector = c.DestConnector,
                        DestConnectorHotspot = c.DestConnectorHotspot,
                        Points = c.Points,
                        SourceConnector = c.SourceConnector,
                        SourceConnectorHotspot = c.SourceConnectorHotspot                 
                    });
            }
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                connList.Add(m_ConnectionInfoList[i].ConnectionVM);
            }

            m_FlowGraphVM.DeleteConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                ConnectionViewModel copy = new ConnectionViewModel();
                copy.DestConnector = m_ConnectionInfoList[i].DestConnector;
                copy.DestConnectorHotspot = m_ConnectionInfoList[i].DestConnectorHotspot;
                copy.Points = m_ConnectionInfoList[i].Points;
                copy.SourceConnector = m_ConnectionInfoList[i].SourceConnector;
                copy.SourceConnectorHotspot = m_ConnectionInfoList[i].SourceConnectorHotspot;

                connList.Add(copy);

                ConnectionInfo inf = m_ConnectionInfoList[i];
                inf.ConnectionVM = copy;
                m_ConnectionInfoList[i] = inf;
            }

            m_FlowGraphVM.AddConnections(connList);
        }

		#endregion //Methods
    }
}
