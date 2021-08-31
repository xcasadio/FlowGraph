using Utils;

namespace NetworkModel
{
    /// <summary>
    /// Defines a network of nodes and connections between the nodes.
    /// </summary>
    public sealed class NetworkViewModel
    {
        #region Internal Data Members

        /// <summary>
        /// The collection of nodes in the network.
        /// </summary>
        private ImpObservableCollection<NodeViewModel> nodesViewModel;

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        private ImpObservableCollection<ConnectionViewModel> connectionsViewModel;

        #endregion Internal Data Members

        /// <summary>
        /// The collection of nodes in the network.
        /// </summary>
        public ImpObservableCollection<NodeViewModel> NodesViewModel => nodesViewModel ?? (nodesViewModel = new ImpObservableCollection<NodeViewModel>());

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        public ImpObservableCollection<ConnectionViewModel> ConnectionsViewModel
        {
            get
            {
                if (connectionsViewModel == null)
                {
                    connectionsViewModel = new ImpObservableCollection<ConnectionViewModel>();
                    connectionsViewModel.ItemsRemoved += connections_ItemsRemoved;
                }

                return connectionsViewModel;
            }
        }

        #region Private Methods

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

        #endregion Private Methods
    }
}
