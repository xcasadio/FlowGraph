﻿using NetworkModel;
using NetworkUI;
using UiTools;

namespace FlowGraphUI.UndoRedo;

class CreateNodeUndoCommand : IUndoCommand
{
    readonly SequenceViewModel _flowGraphVm;
    readonly NodeViewModel _nodeVm;

    public CreateNodeUndoCommand(SequenceViewModel fgvm, NodeViewModel nodeVm)
    {
        _flowGraphVm = fgvm;
        _nodeVm = nodeVm;
    }

    public void Redo()
    {
        _flowGraphVm.AddNode(_nodeVm);
    }

    public void Undo()
    {
        _flowGraphVm.DeleteNode(_nodeVm);
    }

    public override string ToString()
    {
        return $"{_flowGraphVm.Name} : Create node {_nodeVm.Title}";
    }
}

class CreateNodesUndoCommand : IUndoCommand
{
    readonly SequenceViewModel _flowGraphVm;
    readonly IEnumerable<NodeViewModel> _nodesVm;
    readonly List<ConnectionInfo> _connectionInfoList = new();

    public CreateNodesUndoCommand(SequenceViewModel fgv, IEnumerable<NodeViewModel> nodesVm)
    {
        _flowGraphVm = fgv;

        List<ConnectionViewModel> connections = new();
        foreach (var node in nodesVm)
        {
            connections.AddRange(node.AttachedConnections);
        }
        CopyConnections(connections);

        _nodesVm = nodesVm;
    }

    private void CopyConnections(IEnumerable<ConnectionViewModel> connections)
    {
        foreach (ConnectionViewModel c in connections)
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

    public void Redo()
    {
        foreach (var nodeViewModel in _nodesVm)
        {
            _flowGraphVm.Network.Nodes.Add(nodeViewModel);
        }

        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++)
        {
            connList.Add(_connectionInfoList[i].ConnectionVm);
        }

        _flowGraphVm.AddConnections(connList);
    }

    public void Undo()
    {
        List<ConnectionViewModel> connList = new();

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

        _flowGraphVm.DeleteConnections(connList);
        _flowGraphVm.DeleteNodes(_nodesVm);
    }

    public override string ToString()
    {
        return $"Graph[{_flowGraphVm.Name}] : Create nodes";
    }
}

class DeleteNodeUndoCommand : IUndoCommand
{
    readonly SequenceViewModel _flowGraphVm;
    readonly NodeViewModel _nodeVm;
    readonly List<ConnectionInfo> _connectionInfoList = new();

    public DeleteNodeUndoCommand(SequenceViewModel fgv, NodeViewModel nodeVm)
    {
        _flowGraphVm = fgv;
        _nodeVm = nodeVm;
        CopyConnections(_nodeVm.AttachedConnections);
    }

    private void CopyConnections(IEnumerable<ConnectionViewModel> connections)
    {
        foreach (ConnectionViewModel c in connections)
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

    public void Redo()
    {
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++)
        {
            connList.Add(_connectionInfoList[i].ConnectionVm);
        }

        _flowGraphVm.DeleteConnections(connList);
        _flowGraphVm.DeleteNode(_nodeVm);
    }

    public void Undo()
    {
        List<ConnectionViewModel> connList = new();

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

        _flowGraphVm.AddNode(_nodeVm);
        _flowGraphVm.AddConnections(connList);
    }
    public override string ToString()
    {
        return $"{_flowGraphVm.Name} : Delete node {_nodeVm.Title}";
    }
}

class DeleteNodesUndoCommand : IUndoCommand
{
    readonly SequenceViewModel _flowGraphVm;
    readonly IEnumerable<NodeViewModel> _nodesVm;
    readonly List<ConnectionInfo> _connectionInfoList = new();

    public DeleteNodesUndoCommand(SequenceViewModel fgv, IEnumerable<NodeViewModel> nodesVm)
    {
        _flowGraphVm = fgv;

        List<ConnectionViewModel> connections = new();
        foreach (var node in nodesVm)
        {
            connections.AddRange(node.AttachedConnections);
        }
        CopyConnections(connections);

        _nodesVm = nodesVm;
    }

    private void CopyConnections(IEnumerable<ConnectionViewModel> connections)
    {
        foreach (ConnectionViewModel c in connections)
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

    public void Redo()
    {
        List<ConnectionViewModel> connList = new();

        for (int i = 0; i < _connectionInfoList.Count; i++)
        {
            connList.Add(_connectionInfoList[i].ConnectionVm);
        }

        _flowGraphVm.DeleteConnections(connList);

        foreach (var nodeViewModel in _nodesVm)
        {
            _flowGraphVm.Network.Nodes.Remove(nodeViewModel);
        }
    }

    public void Undo()
    {
        List<ConnectionViewModel> connList = new();

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

        foreach (var nodeViewModel in _nodesVm)
        {
            _flowGraphVm.Network.Nodes.Add(nodeViewModel);
        }

        _flowGraphVm.AddConnections(connList);
    }

    public override string ToString()
    {
        return $"Graph[{_flowGraphVm.Name}] : Delete nodes";
    }
}

class PositionNodeUndoCommand : IUndoCommand
{
    internal class NodeDraggingInfo
    {
        public NodeViewModel Node;
        public double StartX, StartY;
        public double EndX, EndY;
    }

    readonly SequenceViewModel _flowGraphVm;
    readonly IEnumerable<NodeDraggingInfo> _nodeInfosVm;

    public PositionNodeUndoCommand(SequenceViewModel fgv, IEnumerable<NodeDraggingInfo> nodeInfosVm)
    {
        _flowGraphVm = fgv;
        _nodeInfosVm = new List<NodeDraggingInfo>(nodeInfosVm);
    }

    public void Redo()
    {
        foreach (NodeDraggingInfo info in _nodeInfosVm)
        {
            info.Node.X = info.EndX;
            info.Node.Y = info.EndY;
        }
    }

    public void Undo()
    {
        foreach (NodeDraggingInfo info in _nodeInfosVm)
        {
            info.Node.X = info.StartX;
            info.Node.Y = info.StartY;
        }
    }

    public override string ToString()
    {
        return $"{_flowGraphVm.Name} : Node position changed";
    }
}

class SelectNodesUndoCommand : IUndoCommand
{
    readonly NetworkView _view;
    readonly IEnumerable<NodeViewModel> _nodesVm;

    public SelectNodesUndoCommand(NetworkView view, IEnumerable<NodeViewModel> nodesVm)
    {
        _view = view;
        _nodesVm = new List<NodeViewModel>(nodesVm);
    }

    public void Redo()
    {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm)
        {
            _view.SelectedNodes.Add(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public void Undo()
    {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm)
        {
            _view.SelectedNodes.Remove(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public override string ToString()
    {
        return "Node selected";
    }
}

class DeselectNodesUndoCommand : IUndoCommand
{
    readonly NetworkView _view;
    readonly IEnumerable<NodeViewModel> _nodesVm;

    public DeselectNodesUndoCommand(NetworkView view, IEnumerable<NodeViewModel> nodesVm)
    {
        _view = view;
        _nodesVm = new List<NodeViewModel>(nodesVm);
    }

    public void Redo()
    {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm)
        {
            _view.SelectedNodes.Remove(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public void Undo()
    {
        _view.IsUndoRegisterEnabled = false;

        foreach (var node in _nodesVm)
        {
            _view.SelectedNodes.Add(node);
        }

        _view.IsUndoRegisterEnabled = true;
    }

    public override string ToString()
    {
        return "Node deselected";
    }
}