using System.Collections.Generic;
using FlowSimulator.UI;
using NetworkModel;
using NetworkUI;

namespace FlowSimulator.Undo
{
    /// <summary>
    /// 
    /// </summary>
    class CreateNodeUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _FlowGraphVM;
        readonly NodeViewModel _NodeVM;

        /// <summary>
        /// 
        /// </summary>
        public CreateNodeUndoCommand(FlowGraphControlViewModel fgvm, NodeViewModel nodeVm)
        {
            _FlowGraphVM = fgvm;
            _NodeVM = nodeVm;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _FlowGraphVM.AddNode(_NodeVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            _FlowGraphVM.DeleteNode(_NodeVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_FlowGraphVM.Sequence.Name} : Create node {_NodeVM.Title}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CreateNodesUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _FlowGraphVM;
        readonly IEnumerable<NodeViewModel> _NodesVM;
        readonly List<ConnectionInfo> _ConnectionInfoList = new List<ConnectionInfo>();

        /// <summary>
        /// 
        /// </summary>
        public CreateNodesUndoCommand(FlowGraphControlViewModel fgv_, IEnumerable<NodeViewModel> nodesVM_)
        {
            _FlowGraphVM = fgv_;

            List<ConnectionViewModel> connections = new List<ConnectionViewModel>();
            foreach (var node in nodesVM_)
            {
                connections.AddRange(node.AttachedConnections);
            }
            CopyConnections(connections);

            _NodesVM = nodesVM_;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections_"></param>
        private void CopyConnections(IEnumerable<ConnectionViewModel> connections_)
        {
            foreach (ConnectionViewModel c in connections_)
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
            _FlowGraphVM.Network.Nodes.AddRange(_NodesVM);

            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < _ConnectionInfoList.Count; i++)
            {
                connList.Add(_ConnectionInfoList[i].ConnectionVM);
            }

            _FlowGraphVM.AddConnections(connList);
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

            _FlowGraphVM.DeleteConnections(connList);
            _FlowGraphVM.DeleteNodes(_NodesVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Graph[{_FlowGraphVM.Sequence.Name}] : Create nodes";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteNodeUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _FlowGraphVM;
        readonly NodeViewModel _NodeVM;
        readonly List<ConnectionInfo> _ConnectionInfoList = new List<ConnectionInfo>();

        /// <summary>
        /// 
        /// </summary>
        public DeleteNodeUndoCommand(FlowGraphControlViewModel fgv_, NodeViewModel nodeVM_)
        {
            _FlowGraphVM = fgv_;
            _NodeVM = nodeVM_;
            CopyConnections(_NodeVM.AttachedConnections);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections_"></param>
        private void CopyConnections(IEnumerable<ConnectionViewModel> connections_)
        {
            foreach (ConnectionViewModel c in connections_)
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
            _FlowGraphVM.DeleteNode(_NodeVM);
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

            _FlowGraphVM.AddNode(_NodeVM);
            _FlowGraphVM.AddConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_FlowGraphVM.Sequence.Name} : Delete node {_NodeVM.Title}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteNodesUndoCommand : IUndoCommand
    {
        readonly FlowGraphControlViewModel _FlowGraphVM;
        readonly IEnumerable<NodeViewModel> _NodesVM;
        readonly List<ConnectionInfo> _ConnectionInfoList = new List<ConnectionInfo>();

        /// <summary>
        /// 
        /// </summary>
        public DeleteNodesUndoCommand(FlowGraphControlViewModel fgv_, IEnumerable<NodeViewModel> nodesVM_)
        {
            _FlowGraphVM = fgv_;

            List<ConnectionViewModel> connections = new List<ConnectionViewModel>();
            foreach (var node in nodesVM_)
            {
                connections.AddRange(node.AttachedConnections);
            }
            CopyConnections(connections);

            _NodesVM = nodesVM_;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections_"></param>
        private void CopyConnections(IEnumerable<ConnectionViewModel> connections_)
        {
            foreach (ConnectionViewModel c in connections_)
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
            _FlowGraphVM.Network.Nodes.RemoveRange(_NodesVM);
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

            _FlowGraphVM.Network.Nodes.AddRange(_NodesVM);
            _FlowGraphVM.AddConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Graph[{_FlowGraphVM.Sequence.Name}] : Delete nodes";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class PositionNodeUndoCommand : IUndoCommand
    {
        internal class NodeDraggingInfo
        {
            public NodeViewModel Node;
            public double StartX, StartY;
            public double EndX, EndY;
        }

        readonly FlowGraphControlViewModel _FlowGraphVM;
        readonly IEnumerable<NodeDraggingInfo> _NodeInfosVM;

        /// <summary>
        /// 
        /// </summary>
        public PositionNodeUndoCommand(FlowGraphControlViewModel fgv_, IEnumerable<NodeDraggingInfo> nodeInfosVM_)
        {
            _FlowGraphVM = fgv_;
            _NodeInfosVM = new List<NodeDraggingInfo>(nodeInfosVM_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            foreach (NodeDraggingInfo info in _NodeInfosVM)
            {
                info.Node.X = info.EndX;
                info.Node.Y = info.EndY;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            foreach (NodeDraggingInfo info in _NodeInfosVM)
            {
                info.Node.X = info.StartX;
                info.Node.Y = info.StartY;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_FlowGraphVM.Sequence.Name} : Node position changed";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class SelectNodesUndoCommand : IUndoCommand
    {
        readonly NetworkView _View;
        readonly IEnumerable<NodeViewModel> _NodesVM;

        /// <summary>
        /// 
        /// </summary>
        public SelectNodesUndoCommand(NetworkView view_, IEnumerable<NodeViewModel> nodesVM_)
        {
            _View = view_;
            _NodesVM = new List<NodeViewModel>(nodesVM_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _View.IsUndoRegisterEnabled = false;

            foreach (var node in _NodesVM)
            {
                _View.SelectedNodes.Add(node);
            }

            _View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            _View.IsUndoRegisterEnabled = false;

            foreach (var node in _NodesVM)
            {
                _View.SelectedNodes.Remove(node);
            }

            _View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Node selected";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class DeselectNodesUndoCommand : IUndoCommand
    {
        readonly NetworkView _View;
        readonly IEnumerable<NodeViewModel> _NodesVM;

        /// <summary>
        /// 
        /// </summary>
        public DeselectNodesUndoCommand(NetworkView view_, IEnumerable<NodeViewModel> nodesVM_)
        {
            _View = view_;
            _NodesVM = new List<NodeViewModel>(nodesVM_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            _View.IsUndoRegisterEnabled = false;

            foreach (var node in _NodesVM)
            {
                _View.SelectedNodes.Remove(node);
            }

            _View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            _View.IsUndoRegisterEnabled = false;

            foreach (var node in _NodesVM)
            {
                _View.SelectedNodes.Add(node);
            }
 
             _View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Node deselected";
        }
    }
}
