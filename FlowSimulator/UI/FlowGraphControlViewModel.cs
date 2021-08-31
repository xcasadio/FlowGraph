using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Xml;
using FlowGraphBase;
using FlowGraphBase.Logger;
using FlowGraphBase.Node;
using FlowGraphUI;
using FlowSimulator.Undo;
using NetworkModel;
using NetworkUI;
using Utils;

namespace FlowSimulator.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class FlowGraphControlViewModel : AbstractModelBase
    {
        public event EventHandler ContextMenuOpened;

        /// <summary>
        /// This is the network that is displayed in the window.
        /// It is the main part of the view-model.
        /// </summary>
        public NetworkViewModel network;

        ///
        /// The current scale at which the content is being viewed.
        /// 
        private double contentScale = 1;

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetX;

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetY;

        ///
        /// The width of the content (in content coordinates).
        /// 
        private double contentWidth = 10000;

        ///
        /// The height of the content (in content coordinates).
        /// 
        private double contentHeight = 10000;

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// view-model so that the value can be shared with the overview window.
        /// 
        private double contentViewportWidth;

        ///
        /// The height of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// view-model so that the value can be shared with the overview window.
        /// 
        private double contentViewportHeight;

        /// <summary>
        /// 
        /// </summary>
        private SequenceBase _Sequence;

        /// <summary>
        /// 
        /// </summary>
        private XmlNode _XmlNodeLoaded;

        /// <summary>
        /// Gets
        /// </summary>
        public string Name => Sequence.Name;

        /// <summary>
        /// Gets
        /// </summary>
        public string Description => Sequence.Description;

        /// <summary>
        /// Gets
        /// </summary>
        public int ID => Sequence.Id;

        /// <summary>
        /// Gets
        /// </summary>
        public UndoRedoManager UndoRedoManager { get; } = new UndoRedoManager();

        /// <summary>
        /// Gets
        /// </summary>
        public SequenceBase Sequence
        {
            get => _Sequence;
            private set
            {
                if (_Sequence != value)
                {
                    if (_Sequence != null)
                    {
                        _Sequence.PropertyChanged -= OnSequencePropertyChanged;
                    }

                    _Sequence = value;
                    _Sequence.PropertyChanged += OnSequencePropertyChanged;
                }
            }
        }

        /// <summary>
        /// This is the network that is displayed in the window.
        /// It is the main part of the view-model.
        /// </summary>
        public NetworkViewModel Network
        {
            get => network;
            set
            {
                network = value;

                OnPropertyChanged("Network");
            }
        }

        ///
        /// The current scale at which the content is being viewed.
        /// 
        public double ContentScale
        {
            get => contentScale;
            set
            {
                contentScale = value;

                OnPropertyChanged("ContentScale");
            }
        }

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetX
        {
            get => contentOffsetX;
            set
            {
                contentOffsetX = value;

                OnPropertyChanged("ContentOffsetX");
            }
        }

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetY
        {
            get => contentOffsetY;
            set
            {
                contentOffsetY = value;

                OnPropertyChanged("ContentOffsetY");
            }
        }

        ///
        /// The width of the content (in content coordinates).
        /// 
        public double ContentWidth
        {
            get => contentWidth;
            set
            {
                contentWidth = value;

                OnPropertyChanged("ContentWidth");
            }
        }

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        public double ContentHeight
        {
            get => contentHeight;
            set
            {
                contentHeight = value;

                OnPropertyChanged("ContentHeight");
            }
        }

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// view-model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportWidth
        {
            get => contentViewportWidth;
            set
            {
                contentViewportWidth = value;

                OnPropertyChanged("ContentViewportWidth");
            }
        }

        ///
        /// The height of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// view-model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportHeight
        {
            get => contentViewportHeight;
            set
            {
                contentViewportHeight = value;

                OnPropertyChanged("ContentViewportHeight");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private FlowGraphControlViewModel()
        {
            Network = new NetworkViewModel();
        }

        /// <summary>
        /// 
        /// </summary>
        public FlowGraphControlViewModel(SequenceBase seq_) :
            this()
        {
            Sequence = seq_;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public FlowGraphControlViewModel(XmlNode node_) : 
            this()
        {
            Load(node_);
        }

        /// <summary>
        /// Called when the user has started to drag out a connector, thus creating a new connection.
        /// </summary>
        public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
        {
            //
            // Create a new connection to add to the view-model.
            //
            var connection = new ConnectionViewModel();

            if (draggedOutConnector.Type == ConnectorType.Output
                || draggedOutConnector.Type == ConnectorType.VariableOutput)
            {
                //
                // The user is dragging out a source connector (an output) and will connect it to a destination connector (an input).
                //
                connection.SourceConnector = draggedOutConnector;
                connection.DestConnectorHotspot = curDragPoint;
            }
            else
            {
                //
                // The user is dragging out a destination connector (an input) and will connect it to a source connector (an output).
                //
                connection.DestConnector = draggedOutConnector;
                connection.SourceConnectorHotspot = curDragPoint;
            }

            //
            // Add the new connection to the view-model.
            //
            Network.Connections.Add(connection);

            return connection;
        }

        /// <summary>
        /// Called to query the application for feedback while the user is dragging the connection.
        /// </summary>
        public void QueryConnnectionFeedback(ConnectorViewModel draggedOutConnector, ConnectorViewModel draggedOverConnector, out object feedbackIndicator, out bool connectionOk)
        {
            if (draggedOutConnector == draggedOverConnector)
            {
                //
                // Can't connect to self!
                // Provide feedback to indicate that this connection is not valid!
                //
                feedbackIndicator = new ConnectionBadIndicator("Can't connect to self");
                connectionOk = false;
            }
            else
            {
                string message = string.Empty;
                var sourceConnector = draggedOutConnector;
                var destConnector = draggedOverConnector;

                //
                // Only allow connections from output connector to input connector (ie each
                // connector must have a different type).
                // Also only allocation from one node to another, never one node back to the same node.
                //
                connectionOk = IsValidConnection(sourceConnector, draggedOverConnector, ref message);

                if (connectionOk)
                {
                    feedbackIndicator = new ConnectionOkIndicator();
                }
                else
                {
                    //
                    // Connectors with the same connector type (eg input & input, or output & output)
                    // can't be connected.
                    // Only connectors with separate connector type (eg input & output).
                    // Provide feedback to indicate that this connection is not valid!
                    //
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
                //
                // The connection was unsuccessful.
                // Maybe the user dragged it out and dropped it in empty space.
                //
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

            //
            // Only allow connections from output connector to input connector (ie each
            // connector must have a different type).
            // Also only allocation from one node to another, never one node back to the same node.
            //
            string dummy = string.Empty;
            bool connectionOk = IsValidConnection(connectorDraggedOut, connectorDraggedOver, ref dummy);

            if (!connectionOk)
            {
                //
                // Connections between connectors that have the same type,
                // eg input -> input or output -> output, are not allowed,
                // Remove the connection.
                //
                Network.Connections.Remove(newConnection);
                return;
            }

            //
            // The user has dragged the connection on top of another valid connector.
            //

            //
            // Remove any existing connection between the same two connectors.
            //
            var existingConnection = FindConnection(connectorDraggedOut, connectorDraggedOver);
            if (existingConnection != null)
            {
                Network.Connections.Remove(existingConnection);
            }

            // Finalize the connection by reordering the source & destination
            // if necessary
            if (newConnection.DestConnector == null)
            {
                if (connectorDraggedOver.SourceSlot.ConnectionType == SlotType.VarInOut)
                {
                    if (newConnection.SourceConnector.SourceSlot.ConnectionType == SlotType.VarIn)
                    {
                        ConnectorViewModel dest = newConnection.SourceConnector;
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
                    ConnectorViewModel dest = newConnection.SourceConnector;
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
                        ConnectorViewModel dest = newConnection.DestConnector;
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
                    ConnectorViewModel dest = newConnection.DestConnector;
                    newConnection.SourceConnector = connectorDraggedOver;
                    newConnection.DestConnector = dest;
                }
            }

            //special case variable from 2 SequenceNode directly connected
            if (newConnection.SourceConnector.SourceSlot is NodeSlotVar
                && newConnection.DestConnector.SourceSlot is NodeSlotVar)
            {
                ConnectorViewModel src = newConnection.SourceConnector;
                newConnection.SourceConnector = newConnection.DestConnector;
                newConnection.DestConnector = src;
            }

            UndoRedoManager.Add(new CreateConnectionUndoCommand(this, newConnection));
        }

        /// <summary>
        /// Check if the 2 slot can be connected
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="message_"></param>
        /// <returns></returns>
        private bool IsValidConnection(ConnectorViewModel a, ConnectorViewModel b, ref string message_)
        {
            bool connectionOk = a.ParentNode != b.ParentNode &&
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
                        message_ = "You can connect only one node into this slot";
                    }
                }
                else if (b.Type == ConnectorType.VariableInput/* || b.Type == ConnectorType.VariableOutput*/)
                {
                    connectionOk = a.Type == ConnectorType.VariableInputOutput && b.AttachedConnections.Count == 0;

                    if (connectionOk == false)
                    {
                        message_ = "You can connect only one node into this slot";
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
                    message_ = "You can not connect because the type is different";
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
            Trace.Assert(connector1.Type != connector2.Type);

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSequencePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// Delete the currently selected nodes from the view-model.
        /// </summary>
        public void DeleteSelectedNodes()
        {
            // Take a copy of the selected nodes list so we can delete nodes while iterating.
            var nodesCopy = Network.Nodes.ToArray();

            List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
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
        public void DeleteNode(NodeViewModel node, bool saveUndo_ = false)
        {
            if (saveUndo_)
            {
                UndoRedoManager.Add(new DeleteNodeUndoCommand(this, node));
            }

            Network.Connections.RemoveRange(node.AttachedConnections);
            Network.Nodes.Remove(node);
        }

        /// <summary>
        /// Delete the nodes from the view-model.
        /// Also deletes any connections to or from the node.
        /// </summary>
        public void DeleteNodes(IEnumerable<NodeViewModel> nodes_, bool saveUndo_ = false)
        {
            if (saveUndo_)
            {
                UndoRedoManager.Add(new DeleteNodesUndoCommand(this, nodes_));
            }

            foreach (var node in nodes_)
            {
                Network.Connections.RemoveRange(node.AttachedConnections);
            }
            
            Network.Nodes.RemoveRange(nodes_);
        }

        /// <summary>
        /// add a node to the view-model.
        /// </summary>
        public void AddNode(NodeViewModel node_, bool saveUndo_ = false)
        {
            if (saveUndo_)
            {
                UndoRedoManager.Add(new CreateNodeUndoCommand(this, node_));
            }

            Network.Nodes.Add(node_);
        }

        /// <summary>
        /// Create a node and add it to the view-model.
        /// </summary>
        public NodeViewModel CreateNode(SequenceNode node_, Point nodeLocation, bool centerNode)
        {
            NodeViewModel node = new NodeViewModel(node_)
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
                EventHandler<EventArgs> sizeChangedEventHandler = null;
                sizeChangedEventHandler =
                    delegate
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
                        node.SizeChanged -= sizeChangedEventHandler;
                    };

                //
                // Now we hook the SizeChanged event so the anonymous method is called later
                // when the size of the node has actually been determined.
                //
                node.SizeChanged += sizeChangedEventHandler;
            }

            AddNode(node, true);

            return node;
        }

        /// <summary>
        /// Copy all nodes in it_ and add it to Network.Nodes
        /// </summary>
        /// <param name="it_"></param>
        /// <param name="centerLocation_"></param>
        /// <returns></returns>
        public IEnumerable<NodeViewModel> CopyNodes(IEnumerable<NodeViewModel> it_) 
        {
            List<NodeViewModel> newNodes = new List<NodeViewModel>();

            foreach (NodeViewModel node in it_)
            {
                NodeViewModel newNode = node.Copy();
                newNode.X += 40;
                newNode.Y += 40;
                newNode.IsSelected = false;
                newNodes.Add(newNode);
            }

            UndoRedoManager.Add(new CreateNodesUndoCommand(this, newNodes));
            Network.Nodes.AddRange(newNodes);

            return newNodes;
        }

        /// <summary>
        /// Utility method to add a connection from the view-model.
        /// </summary>
        public void AddConnection(ConnectionViewModel connection, bool saveUndo_ = false)
        {
            if (saveUndo_)
            {
                UndoRedoManager.Add(new CreateConnectionUndoCommand(this, connection));
            }

            Network.Connections.Add(connection);
        }

        /// <summary>
        /// Utility method to delete a connection from the view-model.
        /// </summary>
        public void DeleteConnection(ConnectionViewModel connection, bool saveUndo_ = false)
        {
            if (saveUndo_)
            {
                UndoRedoManager.Add(new DeleteConnectionUndoCommand(this, connection));
            }

            Network.Connections.Remove(connection);
        }

        /// <summary>
        /// Utility method to add a connection list from the view-model.
        /// </summary>
        public void AddConnections(IEnumerable<ConnectionViewModel> connections, bool saveUndo_ = false)
        {
            if (saveUndo_)
            {
                UndoRedoManager.Add(new CreateConnectionsUndoCommand(this, connections));
            }

            Network.Connections.AddRange(connections);
        }

        /// <summary>
        /// Utility method to delete a connection list from the view-model.
        /// </summary>
        public void DeleteConnections(IEnumerable<ConnectionViewModel> connections, bool saveUndo_ = false)
        {
            if (saveUndo_)
            {
                UndoRedoManager.Add(new DeleteConnectionsUndoCommand(this, connections));
            }

            Network.Connections.RemoveRange(connections);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InitialNodeFromNewFunction()
        {
            // Function already contains nodes when it is created
            // so we need to create the corresponding NodeViewModel for each node
            if (Sequence is SequenceFunction
                && Network.Nodes.Count == 0)
            {
                IEnumerator<SequenceNode> it = Sequence.Nodes.GetEnumerator();
                int i = 0;

                while (it.MoveNext())
                {
                    NodeViewModel nodeVM = new NodeViewModel(it.Current)
                    {
                        X = 50 + i * 150,
                        Y = 50
                    };
                    Network.Nodes.Add(nodeVM);
                    i++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateSequence()
        {
            Sequence.RemoveAllNodes();

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
                Sequence.AddNode(node.SeqNode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            try
            {
                int version = int.Parse(node_.Attributes["version"].Value);

                int graphId = int.Parse(node_.Attributes["id"].Value);
                Sequence = GraphDataManager.Instance.GetById(graphId);

                foreach (SequenceNode node in Sequence.Nodes)
                {
                    XmlNode nodeNode = node_.SelectSingleNode("NodeList/Node[@id='" + node.Id + "']");

                    int versionNode = int.Parse(nodeNode.Attributes["version"].Value);

                    if (nodeNode != null)
                    {
                        NodeViewModel nodeVm = new NodeViewModel(node)
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

                foreach (XmlNode linkNode in node_.SelectNodes("ConnectionList/Connection"))
                {
                    int versionLink = int.Parse(linkNode.Attributes["version"].Value);

                    ConnectionViewModel cvm = new ConnectionViewModel();
                    NodeViewModel srcNode = GetNodeVMBySequenceID(int.Parse(linkNode.Attributes["srcNodeID"].Value));
                    NodeViewModel destNode = GetNodeVMBySequenceID(int.Parse(linkNode.Attributes["destNodeID"].Value));
                    cvm.SourceConnector = srcNode.GetConnectorFromSlotId(int.Parse(linkNode.Attributes["srcNodeSlotID"].Value));
                    cvm.DestConnector = destNode.GetConnectorFromSlotId(int.Parse(linkNode.Attributes["destNodeSlotID"].Value));
                    Network.Connections.Add(cvm);
                }

                _XmlNodeLoaded = node_;
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        private NodeViewModel GetNodeVMBySequenceID(int seqId_)
        {
            foreach (NodeViewModel n in Network.Nodes)
            {
                if (n.SeqNode.Id == seqId_)
                {
                    return n;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Save(XmlNode node_)
        {
            const int version = 1;
            const int versionNode = 1;

            XmlNode graphNode = node_.SelectSingleNode("Graph[@id='" + ID + "']");
            graphNode.AddAttribute("designerVersion", version.ToString());

            //save all nodes
            foreach (NodeViewModel nodeVM in Network.Nodes)
            {
                XmlNode nodeNode = graphNode.SelectSingleNode("NodeList/Node[@id='" + nodeVM.SeqNode.Id + "']");

                nodeNode.AddAttribute("designerVersion", versionNode.ToString());

                nodeNode.AddAttribute("x", nodeVM.X.ToString());
                nodeNode.AddAttribute("y", nodeVM.Y.ToString());
                nodeNode.AddAttribute("z", nodeVM.ZIndex.ToString());
            }
        }

        readonly List<PositionNodeUndoCommand.NodeDraggingInfo> _CachedNodesDraggingList = new List<PositionNodeUndoCommand.NodeDraggingInfo>(5);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnNodeDragStarted(NetworkView sender, NodeDragStartedEventArgs e)
        {
            _CachedNodesDraggingList.Clear();

            foreach (NodeViewModel node in e.Nodes)
            {
                _CachedNodesDraggingList.Add(new PositionNodeUndoCommand.NodeDraggingInfo { Node = node, StartX = node.X, StartY = node.Y });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnNodeDragCompleted(NetworkView sender, NodeDragCompletedEventArgs e)
        {
            foreach (PositionNodeUndoCommand.NodeDraggingInfo info in _CachedNodesDraggingList)
            {
                info.EndX = info.Node.X;
                info.EndY = info.Node.Y;
            }

            UndoRedoManager.Add(new PositionNodeUndoCommand(this, _CachedNodesDraggingList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes_"></param>
        public void OnNodesSelectedChanged(NetworkView view_, IEnumerable<NodeViewModel> nodes_)
        {
            UndoRedoManager.Add(new SelectNodesUndoCommand(view_, nodes_));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes_"></param>
        public void OnNodesDeselectedChanged(NetworkView view_, IEnumerable<NodeViewModel> nodes_)
        {
            UndoRedoManager.Add(new DeselectNodesUndoCommand(view_, nodes_));
        }
    }
}
