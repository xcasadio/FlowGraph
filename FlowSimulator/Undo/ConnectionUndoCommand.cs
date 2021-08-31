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

        FlowGraphControlViewModel _FlowGraphVM;

        ConnectionViewModel _ConnectionVM;
        ConnectorViewModel _DestConnector;
        Point _DestConnectorHotspot;
        PointCollection _Points;
        ConnectorViewModel _SourceConnector;
        Point _SourceConnectorHotspot;

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public CreateConnectionUndoCommand(FlowGraphControlViewModel fgvm, ConnectionViewModel connectionVm)
        {
            _FlowGraphVM = fgvm;
            //_ConnectionVM = connectionVM_.Copy();
            _DestConnector = connectionVm.DestConnector;
            _DestConnectorHotspot = connectionVm.DestConnectorHotspot;
            _Points = connectionVm.Points;
            _SourceConnector = connectionVm.SourceConnector;
            _SourceConnectorHotspot = connectionVm.SourceConnectorHotspot;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _FlowGraphVM.AddConnection(_ConnectionVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            ConnectionViewModel copy = new ConnectionViewModel();
            copy.DestConnector = _DestConnector;
            copy.DestConnectorHotspot = _DestConnectorHotspot;
            copy.Points = _Points;
            copy.SourceConnector = _SourceConnector;
            copy.SourceConnectorHotspot = _SourceConnectorHotspot;

            _FlowGraphVM.DeleteConnection(copy);
            _ConnectionVM = copy;

            //_FlowGraphVM.DeleteConnection(_ConnectionVM);
        }

		#endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel _FlowGraphVM;

        ConnectionViewModel _ConnectionVM;
        ConnectorViewModel _DestConnector;
        Point _DestConnectorHotspot;
        PointCollection _Points;
        ConnectorViewModel _SourceConnector;
        Point _SourceConnectorHotspot;

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DeleteConnectionUndoCommand(FlowGraphControlViewModel fgv_, ConnectionViewModel connectionVM_)
        {
            _FlowGraphVM = fgv_;
            //_ConnectionVM = connectionVM_.Copy();
            _DestConnector = connectionVM_.DestConnector;
            _DestConnectorHotspot = connectionVM_.DestConnectorHotspot;
            _Points = connectionVM_.Points;
            _SourceConnector = connectionVM_.SourceConnector;
            _SourceConnectorHotspot = connectionVM_.SourceConnectorHotspot;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _FlowGraphVM.DeleteConnection(_ConnectionVM);
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            ConnectionViewModel copy = new ConnectionViewModel();
            copy.DestConnector = _DestConnector;
            copy.DestConnectorHotspot = _DestConnectorHotspot;
            copy.Points = _Points;
            copy.SourceConnector = _SourceConnector;
            copy.SourceConnectorHotspot = _SourceConnectorHotspot;

            _FlowGraphVM.AddConnection(copy);
            _ConnectionVM = copy;
        }

		#endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class CreateConnectionsUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel _FlowGraphVM;
        IEnumerable<ConnectionViewModel> _ConnectionsVM;

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public CreateConnectionsUndoCommand(FlowGraphControlViewModel fgv_, IEnumerable<ConnectionViewModel> connectionsVM_)
        {
            _FlowGraphVM = fgv_;
            _ConnectionsVM = connectionsVM_;
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _FlowGraphVM.AddConnections(_ConnectionsVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            _FlowGraphVM.DeleteConnections(_ConnectionsVM);
        }

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionsUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel _FlowGraphVM;
        List<ConnectionInfo> _ConnectionInfoList = new List<ConnectionInfo>();

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DeleteConnectionsUndoCommand(FlowGraphControlViewModel fgv_, IEnumerable<ConnectionViewModel> connectionsVM_)
        {
            _FlowGraphVM = fgv_;

            foreach (ConnectionViewModel c in connectionsVM_)
            {
                _ConnectionInfoList.Add(new ConnectionInfo
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

            for (int i = 0; i < _ConnectionInfoList.Count; i++)
            {
                connList.Add(_ConnectionInfoList[i].ConnectionVM);
            }

            _FlowGraphVM.DeleteConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < _ConnectionInfoList.Count; i++)
            {
                ConnectionViewModel copy = new ConnectionViewModel();
                copy.DestConnector = _ConnectionInfoList[i].DestConnector;
                copy.DestConnectorHotspot = _ConnectionInfoList[i].DestConnectorHotspot;
                copy.Points = _ConnectionInfoList[i].Points;
                copy.SourceConnector = _ConnectionInfoList[i].SourceConnector;
                copy.SourceConnectorHotspot = _ConnectionInfoList[i].SourceConnectorHotspot;

                connList.Add(copy);

                ConnectionInfo inf = _ConnectionInfoList[i];
                inf.ConnectionVM = copy;
                _ConnectionInfoList[i] = inf;
            }

            _FlowGraphVM.AddConnections(connList);
        }

		#endregion //Methods
    }
}
