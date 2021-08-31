using System;
using System.ComponentModel;
using System.Windows;
using FlowGraphBase.Node;
using Utils;

namespace NetworkModel
{
    /// <summary>
    /// Defines a connector (aka connection point) that can be attached to a node and is used to connect the node to another node.
    /// </summary>
    public sealed class ConnectorViewModel : AbstractModelBase
    {
        /// <summary>
        /// Event raised when the connector hotspot has been updated.
        /// </summary>
        public event EventHandler<EventArgs> HotspotUpdated;

        /// <summary>
        /// The connections that are attached to this connector, or null if no connections are attached.
        /// </summary>
        private ImpObservableCollection<ConnectionViewModel> attachedConnections;

        /// <summary>
        /// The hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        private Point hotspot;

        /// <summary>
        /// The name of the connector.
        /// </summary>
        public string Name
        {
            get => SourceSlot.Text;
            set => SourceSlot.Text = value;
        }

        /// <summary>
        /// Defines the type of the connector.
        /// </summary>
        public ConnectorType Type
        {
            get
            {
                if (SourceSlot == null
                    || ParentNode == null)
                {
                    return ConnectorType.Undefined;
                }

                switch (SourceSlot.ConnectionType)
                {
                    case SlotType.NodeIn:
                        return ConnectorType.Input;

                    case SlotType.NodeOut:
                        return ConnectorType.Output;

                    case SlotType.VarIn:
                        return ConnectorType.VariableInput;

                    case SlotType.VarOut:
                        return ConnectorType.VariableOutput;

                    case SlotType.VarInOut:
                        return ConnectorType.VariableInputOutput;
                }

                return ConnectorType.Undefined;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NodeSlot SourceSlot
        {
            get;
        }

        /// <summary>
        /// Returns 'true' if the connector connected to another node.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                foreach (var connection in AttachedConnections)
                {
                    if (connection.SourceConnector != null &&
                        connection.DestConnector != null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Returns 'true' if a connection is attached to the connector.
        /// The other end of the connection may or may not be attached to a node.
        /// </summary>
        public bool IsConnectionAttached => AttachedConnections.Count > 0;

        /// <summary>
        /// The connections that are attached to this connector, or null if no connections are attached.
        /// </summary>
        public ImpObservableCollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                if (attachedConnections == null)
                {
                    attachedConnections = new ImpObservableCollection<ConnectionViewModel>();
                    attachedConnections.ItemsAdded += attachedConnections_ItemsAdded;
                    attachedConnections.ItemsRemoved += attachedConnections_ItemsRemoved;
                }

                return attachedConnections;
            }
        }

        /// <summary>
        /// The parent node that the connector is attached to, or null if the connector is not attached to any node.
        /// </summary>
        public NodeViewModel ParentNode
        {
            get;
            internal set;
        }

        /// <summary>
        /// The hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        public Point Hotspot
        {
            get => hotspot;
            set
            {
                if (hotspot == value)
                {
                    return;
                }

                hotspot = value;

                OnHotspotUpdated();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public ConnectorViewModel(NodeSlot slot)
        {
            if (slot == null)
            {
                throw new InvalidOperationException("ConnectorViewModel() NodeSlot is null");
            }

            SourceSlot = slot;
            SourceSlot.PropertyChanged += OnSlotPropertyChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSlotPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Text":
                    OnPropertyChanged("Name");
                    break;
            }
        }

        /// <summary>
        /// Debug checking to ensure that no connection is added to the list twice.
        /// </summary>
        private void attachedConnections_ItemsAdded(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectionViewModel connection in e.Items)
            {
                connection.ConnectionChanged += connection_ConnectionChanged;
            }

            if ((AttachedConnections.Count - e.Items.Count) == 0)
            {
                // The first connection has been added, notify the data-binding system that
                // 'IsConnected' should be re-evaluated.
                OnPropertyChanged("IsConnectionAttached");
                OnPropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// Event raised when connections have been removed from the connector.
        /// </summary>
        private void attachedConnections_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectionViewModel connection in e.Items)
            {
                connection.ConnectionChanged -= connection_ConnectionChanged;
            }

            if (AttachedConnections.Count == 0)
            {
                // No longer connected to anything, notify the data-binding system that
                // 'IsConnected' should be re-evaluated.
                OnPropertyChanged("IsConnectionAttached");
                OnPropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// Event raised when a connection attached to the connector has changed.
        /// </summary>
        private void connection_ConnectionChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("IsConnectionAttached");
            OnPropertyChanged("IsConnected");
        }

        /// <summary>
        /// Called when the connector hotspot has been updated.
        /// </summary>
        private void OnHotspotUpdated()
        {
            OnPropertyChanged("Hotspot");

            HotspotUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
