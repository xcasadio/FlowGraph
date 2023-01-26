using System.Windows;

namespace NetworkUI
{
    /// <summary>
    /// Arguments for event raised when the user starts to drag a connector out from a node.
    /// </summary>
    internal class ConnectorItemDragStartedEventArgs : RoutedEventArgs
    {
        internal ConnectorItemDragStartedEventArgs(RoutedEvent routedEvent, object source) :
            base(routedEvent, source)
        {
        }

        /// <summary>
        /// Cancel dragging out of the connector.
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Defines the event handler for ConnectorDragStarted events.
    /// </summary>
    internal delegate void ConnectorItemDragStartedEventHandler(object sender, ConnectorItemDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a node in the network.
    /// </summary>
    internal class ConnectorItemDraggingEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The amount the connector has been dragged horizontally.
        /// </summary>
        private readonly double _horizontalChange;

        /// <summary>
        /// The amount the connector has been dragged vertically.
        /// </summary>
        private readonly double _verticalChange;

        public ConnectorItemDraggingEventArgs(RoutedEvent routedEvent, object source, double horizontalChange, double verticalChange) :
            base(routedEvent, source)
        {
            this._horizontalChange = horizontalChange;
            this._verticalChange = verticalChange;
        }

        /// <summary>
        /// The amount the node has been dragged horizontally.
        /// </summary>
        public double HorizontalChange => _horizontalChange;

        /// <summary>
        /// The amount the node has been dragged vertically.
        /// </summary>
        public double VerticalChange => _verticalChange;
    }

    /// <summary>
    /// Defines the event handler for ConnectorDragStarted events.
    /// </summary>
    internal delegate void ConnectorItemDraggingEventHandler(object sender, ConnectorItemDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a connector.
    /// </summary>
    internal class ConnectorItemDragCompletedEventArgs : RoutedEventArgs
    {
        public ConnectorItemDragCompletedEventArgs(RoutedEvent routedEvent, object source) :
            base(routedEvent, source)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for ConnectorDragCompleted events.
    /// </summary>
    internal delegate void ConnectorItemDragCompletedEventHandler(object sender, ConnectorItemDragCompletedEventArgs e);

}
