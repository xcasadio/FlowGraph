using Utils;

namespace NetworkModel
{
    /// <summary>
    /// Defines a network of nodes and connections between the nodes.
    /// </summary>
    public sealed class NetworkViewModel
    {
        /// <summary>
        /// The collection of nodes in the network.
        /// </summary>
        private ImpObservableCollection<NodeViewModel> _nodesViewModel;

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        private ImpObservableCollection<ConnectionViewModel> _connectionsViewModel;

        /// <summary>
        /// The collection of nodes in the network.
        /// </summary>
        public ImpObservableCollection<NodeViewModel> Nodes => _nodesViewModel ?? (_nodesViewModel = new ImpObservableCollection<NodeViewModel>());

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        public ImpObservableCollection<ConnectionViewModel> Connections
        {
            get
            {
                if (_connectionsViewModel == null)
                {
                    _connectionsViewModel = new ImpObservableCollection<ConnectionViewModel>();
                    _connectionsViewModel.ItemsRemoved += connections_ItemsRemoved;
                }

                return _connectionsViewModel;
            }
        }

        /// <summary>
        /// Event raised then Connections have been removed.
        /// </summary>
        private void connections_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectionViewModel connection in e.Items)
            {
                connection.SourceConnector = null;
                connection.DestConnector = null;
            }
        }
    }
}
