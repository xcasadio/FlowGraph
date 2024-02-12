using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FlowGraph;

namespace NetworkModel
{
    /// <summary>
    /// Defines a network of nodes and connections between the nodes.
    /// </summary>
    public sealed class NetworkViewModel
    {
        private readonly SequenceBase _sequence;

        /// <summary>
        /// The collection of nodes in the network.
        /// </summary>
        public ObservableCollection<NodeViewModel> Nodes { get; } = new();

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        public ObservableCollection<ConnectionViewModel> Connections { get; } = new();

        public NetworkViewModel(SequenceBase sequence)
        {
            _sequence = sequence;

            foreach (var sequenceNode in sequence.Nodes)
            {
                Nodes.Add(new NodeViewModel(sequenceNode));
            }

            foreach (var sequenceNode in sequence.Nodes)
            {
                foreach (var slotOut in sequenceNode.SlotConnectorOut)
                {
                    foreach (var connectedNode in slotOut.ConnectedNodes)
                    {
                        var connectionViewModel = new ConnectionViewModel();
                        connectionViewModel.SourceConnector = GetConnectorViewModel(sequenceNode.Id, slotOut.Id);
                        connectionViewModel.DestConnector = GetConnectorViewModel(connectedNode.Node.Id, connectedNode.Id);
                        connectionViewModel.SourceConnectorHotspot = connectionViewModel.SourceConnector.Hotspot;
                        connectionViewModel.DestConnectorHotspot = connectionViewModel.DestConnector.Hotspot;
                        Connections.Add(connectionViewModel);
                    }
                }
            }

            Nodes.CollectionChanged += OnNodeCollectionChanged;
            Connections.CollectionChanged += ConnectionsViewModelOnCollectionChanged;
        }

        private ConnectorViewModel GetConnectorViewModel(int sequenceNodeId, int slotId)
        {
            foreach (var nodeViewModel in Nodes)
            {
                if (nodeViewModel.SeqNode.Id == sequenceNodeId)
                {
                    return nodeViewModel.GetConnectorFromSlotId(slotId);
                }
            }

            return null;
        }

        private void OnNodeCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NodeViewModel nodeViewModel in e.NewItems)
                {
                    _sequence.AddNode(nodeViewModel.SeqNode);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NodeViewModel nodeViewModel in e.OldItems)
                {
                    _sequence.RemoveNode(nodeViewModel.SeqNode);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _sequence.RemoveAllNodes();
            }
        }

        private void ConnectionsViewModelOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Reset)
            {
                connections_ItemsRemoved(sender, e.OldItems);
            }
        }

        private void connections_ItemsRemoved(object? sender, IList newItems)
        {
            foreach (ConnectionViewModel connection in newItems)
            {
                connection.SourceConnector = null;
                connection.DestConnector = null;
            }
        }
    }
}
