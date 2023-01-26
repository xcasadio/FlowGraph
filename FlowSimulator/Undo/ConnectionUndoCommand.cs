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
        public ConnectionViewModel ConnectionVm;
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
        readonly FlowGraphControlViewModel _flowGraphVm;

        ConnectionViewModel _connectionVm;
        readonly ConnectorViewModel _destConnector;
        readonly Point _destConnectorHotspot;
        readonly PointCollection _points;
        readonly ConnectorViewModel _sourceConnector;
        readonly Point _sourceConnectorHotspot;

        /// <summary>
        /// 
        /// </summary>
        public CreateConnectionUndoCommand(FlowGraphControlViewModel fgvm, ConnectionViewModel connectionVm)
        {
            _flowGraphVm = fgvm;
            //_ConnectionVM = connectionVM_.Copy();
            _destConnector = connectionVm.DestConnector;
            _destConnectorHotspot = connectionVm.DestConnectorHotspot;
            _points = connectionVm.Points;
            _sourceConnector = connectionVm.SourceConnector;
            _sourceConnectorHotspot = connectionVm.SourceConnectorHotspot;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _flowGraphVm.AddConnection(_connectionVm);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            ConnectionViewModel copy = new ConnectionViewModel
            {
                DestConnector = _destConnector,
                DestConnectorHotspot = _destConnectorHotspot,
                Points = _points,
                SourceConnector = _sourceConnector,
                SourceConnectorHotspot = _sourceConnectorHotspot
            };

            _flowGraphVm.DeleteConnection(copy);
            _connectionVm = copy;

            //_FlowGraphVM.DeleteConnection(_ConnectionVM);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _flowGraphVm;

        ConnectionViewModel _connectionVm;
        readonly ConnectorViewModel _destConnector;
        readonly Point _destConnectorHotspot;
        readonly PointCollection _points;
        readonly ConnectorViewModel _sourceConnector;
        readonly Point _sourceConnectorHotspot;

        /// <summary>
        /// 
        /// </summary>
        public DeleteConnectionUndoCommand(FlowGraphControlViewModel fgv, ConnectionViewModel connectionVm)
        {
            _flowGraphVm = fgv;
            //_ConnectionVM = connectionVM_.Copy();
            _destConnector = connectionVm.DestConnector;
            _destConnectorHotspot = connectionVm.DestConnectorHotspot;
            _points = connectionVm.Points;
            _sourceConnector = connectionVm.SourceConnector;
            _sourceConnectorHotspot = connectionVm.SourceConnectorHotspot;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _flowGraphVm.DeleteConnection(_connectionVm);

        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            ConnectionViewModel copy = new ConnectionViewModel
            {
                DestConnector = _destConnector,
                DestConnectorHotspot = _destConnectorHotspot,
                Points = _points,
                SourceConnector = _sourceConnector,
                SourceConnectorHotspot = _sourceConnectorHotspot
            };

            _flowGraphVm.AddConnection(copy);
            _connectionVm = copy;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CreateConnectionsUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _flowGraphVm;
        readonly IEnumerable<ConnectionViewModel> _connectionsVm;

        /// <summary>
        /// 
        /// </summary>
        public CreateConnectionsUndoCommand(FlowGraphControlViewModel fgv, IEnumerable<ConnectionViewModel> connectionsVm)
        {
            _flowGraphVm = fgv;
            _connectionsVm = connectionsVm;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _flowGraphVm.AddConnections(_connectionsVm);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            _flowGraphVm.DeleteConnections(_connectionsVm);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteConnectionsUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _flowGraphVm;
        readonly List<ConnectionInfo> _connectionInfoList = new List<ConnectionInfo>();

        /// <summary>
        /// 
        /// </summary>
        public DeleteConnectionsUndoCommand(FlowGraphControlViewModel fgv, IEnumerable<ConnectionViewModel> connectionsVm)
        {
            _flowGraphVm = fgv;

            foreach (ConnectionViewModel c in connectionsVm)
            {
                _connectionInfoList.Add(new ConnectionInfo
                {
                    ConnectionVm = null,
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

            for (int i = 0; i < _connectionInfoList.Count; i++)
            {
                connList.Add(_connectionInfoList[i].ConnectionVm);
            }

            _flowGraphVm.DeleteConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < _connectionInfoList.Count; i++)
            {
                ConnectionViewModel copy = new ConnectionViewModel
                {
                    DestConnector = _connectionInfoList[i].DestConnector,
                    DestConnectorHotspot = _connectionInfoList[i].DestConnectorHotspot,
                    Points = _connectionInfoList[i].Points,
                    SourceConnector = _connectionInfoList[i].SourceConnector,
                    SourceConnectorHotspot = _connectionInfoList[i].SourceConnectorHotspot
                };

                connList.Add(copy);

                ConnectionInfo inf = _connectionInfoList[i];
                inf.ConnectionVm = copy;
                _connectionInfoList[i] = inf;
            }

            _flowGraphVm.AddConnections(connList);
        }
    }
}
