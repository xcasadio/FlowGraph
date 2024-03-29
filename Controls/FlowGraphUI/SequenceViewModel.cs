﻿using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Nodes;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Nodes;
using FlowGraphUI.UndoRedo;
using NetworkModel;
using NetworkUI;
using Newtonsoft.Json.Linq;
using UiTools;
using Utils;

namespace FlowGraphUI;

public class SequenceViewModel : AbstractModelBase
{
    public event EventHandler ContextMenuOpened;

    /// <summary>
    /// This is the NetworkViewModel that is displayed in the window.
    /// It is the main part of the view-model.
    /// </summary>
    public NetworkViewModel NetworkViewModel;

    /// The current scale at which the content is being viewed.
    private double _contentScale = 1;

    /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
    private double _contentOffsetX;

    /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
    private double _contentOffsetY;

    /// The width of the content (in content coordinates).
    private double _contentWidth = 10000;

    /// The height of the content (in content coordinates).
    private double _contentHeight = 10000;

    /// The width of the viewport onto the content (in content coordinates).
    /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
    /// view-model so that the value can be shared with the overview window.
    private double _contentViewportWidth;

    /// The height of the viewport onto the content (in content coordinates).
    /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
    /// view-model so that the value can be shared with the overview window.
    private double _contentViewportHeight;

    private readonly SequenceBase _sequence;

    public SequenceBase SequenceBase => _sequence;

    public string? Name
    {
        get => _sequence.Name;
        set
        {
            if (string.Equals(_sequence.Name, value))
            {
                return;
            }

            _sequence.Name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public string? Description
    {
        get => _sequence.Description;
        set
        {
            if (string.Equals(_sequence.Description, value))
            {
                return;
            }

            _sequence.Description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public int Id => _sequence.Id;

    public UndoRedoManager UndoRedoManager { get; }

    /// <summary>
    /// This is the NetworkViewModel that is displayed in the window.
    /// It is the main part of the view-model.
    /// </summary>
    public NetworkViewModel Network
    {
        get => NetworkViewModel;
        set
        {
            NetworkViewModel = value;

            OnPropertyChanged("Network");
        }
    }

    /// The current scale at which the content is being viewed.
    public double ContentScale
    {
        get => _contentScale;
        set
        {
            _contentScale = value;

            OnPropertyChanged("ContentScale");
        }
    }

    /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
    public double ContentOffsetX
    {
        get => _contentOffsetX;
        set
        {
            _contentOffsetX = value;

            OnPropertyChanged("ContentOffsetX");
        }
    }

    /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
    public double ContentOffsetY
    {
        get => _contentOffsetY;
        set
        {
            _contentOffsetY = value;

            OnPropertyChanged("ContentOffsetY");
        }
    }

    /// The width of the content (in content coordinates).
    public double ContentWidth
    {
        get => _contentWidth;
        set
        {
            _contentWidth = value;

            OnPropertyChanged("ContentWidth");
        }
    }

    /// The heigth of the content (in content coordinates).
    public double ContentHeight
    {
        get => _contentHeight;
        set
        {
            _contentHeight = value;

            OnPropertyChanged("ContentHeight");
        }
    }

    /// The width of the viewport onto the content (in content coordinates).
    /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
    /// view-model so that the value can be shared with the overview window.
    public double ContentViewportWidth
    {
        get => _contentViewportWidth;
        set
        {
            _contentViewportWidth = value;

            OnPropertyChanged("ContentViewportWidth");
        }
    }

    /// The height of the viewport onto the content (in content coordinates).
    /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
    /// view-model so that the value can be shared with the overview window.
    public double ContentViewportHeight
    {
        get => _contentViewportHeight;
        set
        {
            _contentViewportHeight = value;

            OnPropertyChanged("ContentViewportHeight");
        }
    }

    public SequenceViewModel(SequenceBase sequenceBase)
    {
        _sequence = sequenceBase;

        Network = new NetworkViewModel(_sequence);
        UndoRedoManager = new UndoRedoManager(LogManager.Instance);
    }

    /// <summary>
    /// Called when the user has started to drag out a connector, thus creating a new connection.
    /// </summary>
    public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
    {
        var connection = new ConnectionViewModel();

        if (draggedOutConnector.Type == ConnectorType.Output
            || draggedOutConnector.Type == ConnectorType.VariableOutput)
        {
            connection.SourceConnector = draggedOutConnector;
            connection.DestConnectorHotspot = curDragPoint;
        }
        else
        {
            connection.DestConnector = draggedOutConnector;
            connection.SourceConnectorHotspot = curDragPoint;
        }

        Network.Connections.Add(connection);

        return connection;
    }

    /// <summary>
    /// Called to query the application for feedback while the user is dragging the connection.
    /// </summary>
    public void QueryConnectionFeedback(ConnectorViewModel draggedOutConnector, ConnectorViewModel draggedOverConnector, out object feedbackIndicator, out bool connectionOk)
    {
        if (draggedOutConnector == draggedOverConnector)
        {
            feedbackIndicator = new ConnectionBadIndicator("Can't connect to self");
            connectionOk = false;
        }
        else
        {
            var message = string.Empty;
            var sourceConnector = draggedOutConnector;
            var destConnector = draggedOverConnector;

            connectionOk = IsValidConnection(sourceConnector, draggedOverConnector, ref message);

            if (connectionOk)
            {
                feedbackIndicator = new ConnectionOkIndicator();
            }
            else
            {
                feedbackIndicator = new ConnectionBadIndicator(message);
            }
        }
    }

    /// <summary>
    /// Called as the user continues to drag the connection.
    /// </summary>
    public void ConnectionDragging(Point curDragPoint, ConnectionViewModel connection)
    {
        if (connection.DestConnector == null)
        {
            connection.DestConnectorHotspot = curDragPoint;
        }
        else
        {
            connection.SourceConnectorHotspot = curDragPoint;
        }
    }

    /// <summary>
    /// Called when the user has finished dragging out the new connection.
    /// </summary>
    public void ConnectionDragCompleted(
        ConnectionViewModel newConnection,
        ConnectorViewModel connectorDraggedOut,
        ConnectorViewModel connectorDraggedOver)
    {
        if (connectorDraggedOver == null)
        {
            Network.Connections.Remove(newConnection);
            //                 _connectorDraggedOut = connectorDraggedOut;
            //                 _ConnectorDraggedOver = connectorDraggedOver;
            // 
            //                 //Open contextMenu
            //                 if (ContextMenuOpened != null)
            //                 {
            //                     ContextMenuOpened(this, null);
            //                 }

            return;
        }

        var dummy = string.Empty;
        var connectionOk = IsValidConnection(connectorDraggedOut, connectorDraggedOver, ref dummy);

        if (!connectionOk)
        {
            Network.Connections.Remove(newConnection);
            return;
        }

        var existingConnection = FindConnection(connectorDraggedOut, connectorDraggedOver);
        if (existingConnection != null)
        {
            Network.Connections.Remove(existingConnection);
        }

        if (newConnection.DestConnector == null)
        {
            if (connectorDraggedOver.SourceSlot.ConnectionType == SlotType.VarInOut)
            {
                if (newConnection.SourceConnector.SourceSlot.ConnectionType == SlotType.VarIn)
                {
                    var dest = newConnection.SourceConnector;
                    newConnection.SourceConnector = connectorDraggedOver;
                    newConnection.DestConnector = dest;
                }
                else
                {
                    newConnection.DestConnector = connectorDraggedOver;
                }
            }
            else if (connectorDraggedOver.SourceSlot.ConnectionType == SlotType.NodeIn
                     || connectorDraggedOver.SourceSlot.ConnectionType == SlotType.VarIn)
            {
                newConnection.DestConnector = connectorDraggedOver;
            }
            else
            {
                var dest = newConnection.SourceConnector;
                newConnection.SourceConnector = connectorDraggedOver;
                newConnection.DestConnector = dest;
            }
        }
        else // connector source is null
        {
            if (connectorDraggedOver.SourceSlot.ConnectionType == SlotType.VarInOut)
            {
                if (newConnection.DestConnector.SourceSlot.ConnectionType == SlotType.VarIn)
                {
                    newConnection.SourceConnector = connectorDraggedOver;
                }
                else
                {
                    var dest = newConnection.DestConnector;
                    newConnection.DestConnector = connectorDraggedOver;
                    newConnection.SourceConnector = dest;
                }
            }
            else if (connectorDraggedOver.SourceSlot.ConnectionType == SlotType.NodeIn
                     || connectorDraggedOver.SourceSlot.ConnectionType == SlotType.VarIn)
            {
                newConnection.SourceConnector = connectorDraggedOver;
            }
            else
            {
                var dest = newConnection.DestConnector;
                newConnection.SourceConnector = connectorDraggedOver;
                newConnection.DestConnector = dest;
            }
        }

        //special case variable from 2 SequenceNode directly connected
        if (newConnection.SourceConnector.SourceSlot is NodeSlotVar
            && newConnection.DestConnector.SourceSlot is NodeSlotVar)
        {
            (newConnection.SourceConnector, newConnection.DestConnector) = (newConnection.DestConnector, newConnection.SourceConnector);
        }

        newConnection.SourceConnector.SourceSlot.ConnectTo(newConnection.DestConnector.SourceSlot);

        UndoRedoManager.Add(new CreateConnectionUndoCommand(this, newConnection));
    }

    /// <summary>
    /// Check if the 2 slot can be connected
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="message_"></param>
    /// <returns></returns>
    private bool IsValidConnection(ConnectorViewModel a, ConnectorViewModel b, ref string message)
    {
        var connectionOk = a.ParentNode != b.ParentNode &&
                           a.Type != b.Type;

        if (connectionOk)
        {
            //case Variable Input Output
            if (a.Type == ConnectorType.VariableInputOutput)
            {
                connectionOk = b.Type == ConnectorType.VariableInput || b.Type == ConnectorType.VariableOutput;
            }
            else if (b.Type == ConnectorType.VariableInputOutput)
            {
                connectionOk = a.Type == ConnectorType.VariableInput || a.Type == ConnectorType.VariableOutput;
            }
            //case node/variable
            if (a.Type == ConnectorType.Input
                || a.Type == ConnectorType.Output)
            {
                connectionOk = b.Type == ConnectorType.Input || b.Type == ConnectorType.Output;
            }
            else if (b.Type == ConnectorType.Input
                     || b.Type == ConnectorType.Output)
            {
                connectionOk = a.Type == ConnectorType.Input || a.Type == ConnectorType.Output;
            }
            //TODO : test connection object type
            //case Variable Output/Variable
            if (a.Type == ConnectorType.VariableOutput)
            {
                return b.Type == ConnectorType.VariableInputOutput || b.Type == ConnectorType.VariableInput;
            }

            if (b.Type == ConnectorType.VariableOutput)
            {
                return a.Type == ConnectorType.VariableInputOutput || a.Type == ConnectorType.VariableInput;
            }
            //case Variable Input/Variable
            if (a.Type == ConnectorType.VariableInput/* || a.Type == ConnectorType.VariableOutput*/)
            {
                //a.AttachedConnections.Count == 1 because the destination connector is already connected
                connectionOk = b.Type == ConnectorType.VariableInputOutput && a.AttachedConnections.Count == 1;

                if (connectionOk == false)
                {
                    message = "You can connect only one node into this slot";
                }
            }
            else if (b.Type == ConnectorType.VariableInput/* || b.Type == ConnectorType.VariableOutput*/)
            {
                connectionOk = a.Type == ConnectorType.VariableInputOutput && b.AttachedConnections.Count == 0;

                if (connectionOk == false)
                {
                    message = "You can connect only one node into this slot";
                }
            }
        }

        // TODO : check if obsolete with direct connection ??
        // check for variable node connection
        if (connectionOk
            && (
                (a.Type == ConnectorType.VariableInput
                 || a.Type == ConnectorType.VariableOutput
                 || a.Type == ConnectorType.VariableInputOutput)
                &&
                (b.Type == ConnectorType.VariableInput
                 || b.Type == ConnectorType.VariableOutput
                 || b.Type == ConnectorType.VariableInputOutput)
            ))
        {
            connectionOk = VariableTypeInspector.CheckCompatibilityType(a.SourceSlot.VariableType, b.SourceSlot.VariableType);

            if (connectionOk == false)
            {
                message = "You can not connect because the type is different";
            }
        }

        return connectionOk;
    }

    /// <summary>
    /// Retrieve a connection between the two connectors.
    /// Returns null if there is no connection between the connectors.
    /// </summary>
    public ConnectionViewModel FindConnection(ConnectorViewModel connector1, ConnectorViewModel connector2)
    {
        Debug.Assert(connector1.Type != connector2.Type);

        //
        // Figure out which one is the source connector and which one is the
        // destination connector based on their connector types.
        //
        var sourceConnector = connector1.Type == ConnectorType.Output || connector1.Type == ConnectorType.VariableOutput ?
            connector1 : connector2;
        var destConnector = connector1.Type == ConnectorType.Output || connector1.Type == ConnectorType.VariableOutput ?
            connector2 : connector1;

        //
        // Now we can just iterate attached connections of the source
        // and see if it each one is attached to the destination connector.
        //

        foreach (var connection in sourceConnector.AttachedConnections)
        {
            if (connection.DestConnector == destConnector)
            {
                //
                // Found a connection that is outgoing from the source connector
                // and incoming to the destination connector.
                //
                return connection;
            }
        }

        return null;
    }

    /// <summary>
    /// Delete the currently selected nodes from the view-model.
    /// </summary>
    public void DeleteSelectedNodes()
    {
        // Take a copy of the selected nodes list so we can delete nodes while iterating.
        var nodesCopy = Network.Nodes.ToArray();

        var selectedNodes = new List<NodeViewModel>();
        foreach (var node in nodesCopy)
        {
            if (node.IsSelected)
            {
                selectedNodes.Add(node);
            }
        }

        UndoRedoManager.Add(new DeleteNodesUndoCommand(this, selectedNodes));

        foreach (var node in selectedNodes)
        {
            DeleteNode(node);
        }
    }

    /// <summary>
    /// Delete the node from the view-model.
    /// Also deletes any connections to or from the node.
    /// </summary>
    public void DeleteNode(NodeViewModel node, bool saveUndo = false)
    {
        if (saveUndo)
        {
            UndoRedoManager.Add(new DeleteNodeUndoCommand(this, node));
        }

        foreach (var connectionViewModel in node.AttachedConnections)
        {
            Network.Connections.Remove(connectionViewModel);
        }

        Network.Nodes.Remove(node);
    }

    /// <summary>
    /// Delete the nodes from the view-model.
    /// Also deletes any connections to or from the node.
    /// </summary>
    public void DeleteNodes(IEnumerable<NodeViewModel> nodes, bool saveUndo = false)
    {
        if (saveUndo)
        {
            UndoRedoManager.Add(new DeleteNodesUndoCommand(this, nodes));
        }

        foreach (var node in nodes)
        {
            foreach (var connectionViewModel in node.AttachedConnections)
            {
                Network.Connections.Remove(connectionViewModel);
            }
        }

        foreach (var nodeViewModel in Network.Nodes)
        {
            Network.Nodes.Remove(nodeViewModel);
        }
    }

    /// <summary>
    /// add a node to the view-model.
    /// </summary>
    public void AddNode(NodeViewModel node, bool saveUndo = false)
    {
        if (saveUndo)
        {
            UndoRedoManager.Add(new CreateNodeUndoCommand(this, node));
        }

        Network.Nodes.Add(node);
    }

    /// <summary>
    /// Create a node and add it to the view-model.
    /// </summary>
    public NodeViewModel CreateNode(SequenceNode sequenceNode, Point nodeLocation, bool centerNode)
    {
        var node = new NodeViewModel(sequenceNode)
        {
            X = nodeLocation.X,
            Y = nodeLocation.Y
        };

        if (centerNode)
        {
            // 
            // We want to center the node.
            //
            // For this to happen we need to wait until the UI has determined the 
            // size based on the node's data-template.
            //
            // So we define an anonymous method to handle the SizeChanged event for a node.
            //
            // Note: If you don't declare sizeChangedEventHandler before initializing it you will get
            //       an error when you try and unsubscribe the event from within the event handler.
            //
            void SizeChangedEventHandler(object sender, EventArgs e)
            {
                //
                // This event handler will be called after the size of the node has been determined.
                // So we can now use the size of the node to modify its position.
                //
                node.X -= node.Size.Width / 2;
                node.Y -= node.Size.Height / 2;

                //
                // Don't forget to unhook the event, after the initial centering of the node
                // we don't need to be notified again of any size changes.
                //
                node.SizeChanged -= SizeChangedEventHandler;
            }

            //
            // Now we hook the SizeChanged event so the anonymous method is called later
            // when the size of the node has actually been determined.
            //
            node.SizeChanged += SizeChangedEventHandler;
        }

        AddNode(node, true);

        return node;
    }

    public IEnumerable<NodeViewModel> CopyNodes(IEnumerable<NodeViewModel> it)
    {
        var newNodes = new List<NodeViewModel>();

        foreach (var node in it)
        {
            var newNode = node.Copy();
            newNode.X += 40;
            newNode.Y += 40;
            newNode.IsSelected = false;
            newNodes.Add(newNode);
        }

        UndoRedoManager.Add(new CreateNodesUndoCommand(this, newNodes));

        foreach (var nodeViewModel in newNodes)
        {
            Network.Nodes.Add(nodeViewModel);
        }

        return newNodes;
    }

    public void AddConnection(ConnectionViewModel connection, bool saveUndo = false)
    {
        if (saveUndo)
        {
            UndoRedoManager.Add(new CreateConnectionUndoCommand(this, connection));
        }

        Network.Connections.Add(connection);
    }

    public void DeleteConnection(ConnectionViewModel connection, bool saveUndo = false)
    {
        if (saveUndo)
        {
            UndoRedoManager.Add(new DeleteConnectionUndoCommand(this, connection));
        }

        Network.Connections.Remove(connection);
    }

    public void AddConnections(IEnumerable<ConnectionViewModel> connections, bool saveUndo = false)
    {
        if (saveUndo)
        {
            UndoRedoManager.Add(new CreateConnectionsUndoCommand(this, connections));
        }

        foreach (var connectionViewModel in connections)
        {
            Network.Connections.Add(connectionViewModel);
        }
    }

    public void DeleteConnections(IEnumerable<ConnectionViewModel> connections, bool saveUndo = false)
    {
        if (saveUndo)
        {
            UndoRedoManager.Add(new DeleteConnectionsUndoCommand(this, connections));
        }

        foreach (var connectionViewModel in connections)
        {
            Network.Connections.Remove(connectionViewModel);
        }
    }

    public void InitialNodeFromNewFunction()
    {
        // Function already contains nodes when it is created
        // so we need to create the corresponding NodeViewModel for each node
        if (_sequence is SequenceFunction
            && Network.Nodes.Count == 0)
        {
            var it = _sequence.Nodes.GetEnumerator();
            var i = 0;

            while (it.MoveNext())
            {
                var nodeVm = new NodeViewModel(it.Current)
                {
                    X = 50 + i * 150,
                    Y = 50
                };
                Network.Nodes.Add(nodeVm);
                i++;
            }
        }
    }

    public void CreateSequence()
    {
        _sequence.RemoveAllNodes();

        foreach (var node in Network.Nodes)
        {
            node.SeqNode.RemoveAllConnections();
        }

        //connect all nodes
        foreach (var link in Network.Connections)
        {
            if (link.SourceConnector != null
                && link.DestConnector != null)
            {
                if (link.SourceConnector.SourceSlot is NodeSlotVar
                    && link.DestConnector.SourceSlot is NodeSlotVar)
                {
                    //In real time the link is in the right direction
                    if (link.SourceConnector.SourceSlot.ConnectionType == SlotType.VarIn)
                    {
                        link.SourceConnector.SourceSlot.ConnectTo(link.DestConnector.SourceSlot);
                    }
                    //When the sequence is loaded from file time the link is not in the right direction
                    else
                    {
                        link.DestConnector.SourceSlot.ConnectTo(link.SourceConnector.SourceSlot);
                    }

                    continue;
                }

                if (link.DestConnector.SourceSlot.ConnectionType == SlotType.NodeOut
                    || link.DestConnector.SourceSlot.ConnectionType == SlotType.VarOut
                    || link.DestConnector.SourceSlot.ConnectionType == SlotType.VarIn)
                {
                    link.DestConnector.SourceSlot.ConnectTo(link.SourceConnector.SourceSlot);
                    continue;
                }

                if (link.SourceConnector.SourceSlot.ConnectionType == SlotType.NodeOut
                    || link.SourceConnector.SourceSlot.ConnectionType == SlotType.VarOut
                    || link.SourceConnector.SourceSlot.ConnectionType == SlotType.VarIn)
                {
                    link.SourceConnector.SourceSlot.ConnectTo(link.DestConnector.SourceSlot);
                }
            }
        }

        foreach (var node in Network.Nodes)
        {
            _sequence.AddNode(node.SeqNode);
        }
    }
    /*
    public void Load(JsonNode xmlNode)
    {
        try
        {
            var version = int.Parse(xmlNode.Attributes["version"].Value);

            var graphId = int.Parse(xmlNode.Attributes["id"].Value);
            _sequence = GraphDataManager.Instance.GetById(graphId);

            foreach (var sequenceNode in _sequence.Nodes)
            {
                var nodeNode = xmlNode.SelectSingleNode("NodeList/Node[@id='" + sequenceNode.Id + "']");

                if (nodeNode != null)
                {
                    var versionNode = int.Parse(nodeNode.Attributes["version"].Value);

                    var nodeVm = new NodeViewModel(sequenceNode)
                    {
                        X = double.Parse(nodeNode.Attributes["x"].Value),
                        Y = double.Parse(nodeNode.Attributes["y"].Value),
                        ZIndex = int.Parse(nodeNode.Attributes["z"].Value)
                    };
                    Network.Nodes.Add(nodeVm);
                }
                else
                {
                    throw new InvalidOperationException("Can't find node from xml " +
                                                        $"id={nodeNode.Attributes["id"].Value}");
                }
            }

            foreach (XmlNode linkNode in xmlNode.SelectNodes("ConnectionList/Connection"))
            {
                var versionLink = int.Parse(linkNode.Attributes["version"].Value);

                var cvm = new ConnectionViewModel();
                var srcNode = GetNodeVmBySequenceId(int.Parse(linkNode.Attributes["srcNodeID"].Value));
                var destNode = GetNodeVmBySequenceId(int.Parse(linkNode.Attributes["destNodeID"].Value));
                cvm.SourceConnector = srcNode.GetConnectorFromSlotId(int.Parse(linkNode.Attributes["srcNodeSlotID"].Value));
                cvm.DestConnector = destNode.GetConnectorFromSlotId(int.Parse(linkNode.Attributes["destNodeSlotID"].Value));
                Network.Connections.Add(cvm);
            }

            _xmlNodeLoaded = xmlNode;
        }
        catch (System.Exception ex)
        {
            LogManager.Instance.WriteException(ex);
        }
    }*/

    private NodeViewModel GetNodeVmBySequenceId(int seqId)
    {
        foreach (var n in Network.Nodes)
        {
            if (n.SeqNode.Id == seqId)
            {
                return n;
            }
        }

        return null;
    }

    readonly List<PositionNodeUndoCommand.NodeDraggingInfo> _cachedNodesDraggingList = new(5);

    public void OnNodeDragStarted(NetworkView sender, NodeDragStartedEventArgs e)
    {
        _cachedNodesDraggingList.Clear();

        foreach (NodeViewModel node in e.Nodes)
        {
            _cachedNodesDraggingList.Add(new PositionNodeUndoCommand.NodeDraggingInfo { Node = node, StartX = node.X, StartY = node.Y });
        }
    }

    public void OnNodeDragCompleted(NetworkView sender, NodeDragCompletedEventArgs e)
    {
        foreach (var info in _cachedNodesDraggingList)
        {
            info.EndX = info.Node.X;
            info.EndY = info.Node.Y;
        }

        UndoRedoManager.Add(new PositionNodeUndoCommand(this, _cachedNodesDraggingList));
    }

    public void OnNodesSelectedChanged(NetworkView view, IEnumerable<NodeViewModel> nodes)
    {
        UndoRedoManager.Add(new SelectNodesUndoCommand(view, nodes));
    }

    public void OnNodesDeselectedChanged(NetworkView view, IEnumerable<NodeViewModel> nodes)
    {
        UndoRedoManager.Add(new DeselectNodesUndoCommand(view, nodes));
    }
}