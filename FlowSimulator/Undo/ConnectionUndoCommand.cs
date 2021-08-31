using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using FlowSimulator.UI;
using NetworkModel;

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
        readonly FlowGraphControlViewModel _FlowGraphVM;

        ConnectionViewModel _ConnectionVM;
        readonly ConnectorViewModel _DestConnector;
        readonly Point _DestConnectorHotspot;
        readonly PointCollection _Points;
        readonly ConnectorViewModel _SourceConnector;
        readonly Point _SourceConnectorHotspot;

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
            ConnectionViewModel copy = new ConnectionViewModel
            {
                DestConnector = _DestConnector,
                DestConnectorHotspot = _DestConnectorHotspot,
                Points = _Points,
                SourceConnector = _SourceConnector,
                SourceConnectorHotspot = _SourceConnectorHotspot
            };

            _FlowGraphVM.DeleteConnection(copy);
            _ConnectionVM = copy;

            //_FlowGraphVM.DeleteConnection(_ConnectionVM);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _FlowGraphVM;

        ConnectionViewModel _ConnectionVM;
        readonly ConnectorViewModel _DestConnector;
        readonly Point _DestConnectorHotspot;
        readonly PointCollection _Points;
        readonly ConnectorViewModel _SourceConnector;
        readonly Point _SourceConnectorHotspot;

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
            ConnectionViewModel copy = new ConnectionViewModel
            {
                DestConnector = _DestConnector,
                DestConnectorHotspot = _DestConnectorHotspot,
                Points = _Points,
                SourceConnector = _SourceConnector,
                SourceConnectorHotspot = _SourceConnectorHotspot
            };

            _FlowGraphVM.AddConnection(copy);
            _ConnectionVM = copy;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CreateConnectionsUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _FlowGraphVM;
        readonly IEnumerable<ConnectionViewModel> _ConnectionsVM;

        /// <summary>
        /// 
        /// </summary>
        public CreateConnectionsUndoCommand(FlowGraphControlViewModel fgv_, IEnumerable<ConnectionViewModel> connectionsVM_)
        {
            _FlowGraphVM = fgv_;
            _ConnectionsVM = connectionsVM_;
        }

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
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionsUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _FlowGraphVM;
        readonly List<ConnectionInfo> _ConnectionInfoList = new List<ConnectionInfo>();

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
                ConnectionViewModel copy = new ConnectionViewModel
                {
                    DestConnector = _ConnectionInfoList[i].DestConnector,
                    DestConnectorHotspot = _ConnectionInfoList[i].DestConnectorHotspot,
                    Points = _ConnectionInfoList[i].Points,
                    SourceConnector = _ConnectionInfoList[i].SourceConnector,
                    SourceConnectorHotspot = _ConnectionInfoList[i].SourceConnectorHotspot
                };

                connList.Add(copy);

                ConnectionInfo inf = _ConnectionInfoList[i];
                inf.ConnectionVM = copy;
                _ConnectionInfoList[i] = inf;
            }

            _FlowGraphVM.AddConnections(connList);
        }
    }
}
