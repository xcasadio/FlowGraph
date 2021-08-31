using System.Windows;

namespace NetworkUI
{
    /// <summary>
    /// Base class for connection dragging event args.
    /// </summary>
    public class ConnectionDragEventArgs : RoutedEventArgs
    {
        #region Private Data Members

        /// <summary>
        /// The NodeItem or it's DataContext (when non-NULL).
        /// </summary>
        private readonly object node;

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        private readonly object draggedOutConnector;

        /// <summary>
        /// The connector that will be dragged out.
        /// </summary>
        protected object connection;

        #endregion Private Data Members

        /// <summary>
        /// The NodeItem or it's DataContext (when non-NULL).
        /// </summary>
        public object Node => node;

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        public object ConnectorDraggedOut => draggedOutConnector;

        #region Private Methods

        protected ConnectionDragEventArgs(RoutedEvent routedEvent, object source, object node, object connection, object connector) :
            base(routedEvent, source)
        {
            this.node = node;
            draggedOutConnector = connector;
            this.connection = connection;
        }

        #endregion Private Methods
    }

    /// <summary>
    /// Arguments for event raised when the user starts to drag a connection out from a node.
    /// </summary>
    public class ConnectionDragStartedEventArgs : ConnectionDragEventArgs
    {
        /// <summary>
        /// The connection that will be dragged out.
        /// </summary>
        public object Connection
        {
            get => connection;
            set => connection = value;
        }

        #region Private Methods

        internal ConnectionDragStartedEventArgs(RoutedEvent routedEvent, object source, object node, object connector) :
            base(routedEvent, source, node, null, connector)
        {
        }

        #endregion Private Methods
    }

    /// <summary>
    /// Defines the event handler for the ConnectionDragStarted event.
    /// </summary>
    public delegate void ConnectionDragStartedEventHandler(object sender, ConnectionDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a node in the network.
    /// </summary>
    public class QueryConnectionFeedbackEventArgs : ConnectionDragEventArgs
    {
        #region Private Data Members

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        private readonly object draggedOverConnector;

        /// <summary>
        /// Set to 'true' / 'false' to indicate that the connection from the dragged out connection to the dragged over connector is valid.
        /// </summary>
        private bool connectionOk = true;

        /// <summary>
        /// The indicator to display.
        /// </summary>
        private object feedbackIndicator;

        #endregion Private Data Members

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        public object DraggedOverConnector => draggedOverConnector;

        /// <summary>
        /// The connection that will be dragged out.
        /// </summary>
        public object Connection => connection;

        /// <summary>
        /// Set to 'true' / 'false' to indicate that the connection from the dragged out connection to the dragged over connector is valid.
        /// </summary>
        public bool ConnectionOk
        {
            get => connectionOk;
            set => connectionOk = value;
        }

        /// <summary>
        /// The indicator to display.
        /// </summary>
        public object FeedbackIndicator
        {
            get => feedbackIndicator;
            set => feedbackIndicator = value;
        }

        #region Private Methods

        internal QueryConnectionFeedbackEventArgs(RoutedEvent routedEvent, object source, 
            object node, object connection, object connector, object draggedOverConnector) :
            base(routedEvent, source, node, connection, connector)
        {
            this.draggedOverConnector = draggedOverConnector;
        }

        #endregion Private Methods
    }

    /// <summary>
    /// Defines the event handler for the QueryConnectionFeedback event.
    /// </summary>
    public delegate void QueryConnectionFeedbackEventHandler(object sender, QueryConnectionFeedbackEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a node in the network.
    /// </summary>
    public class ConnectionDraggingEventArgs : ConnectionDragEventArgs
    {
        /// <summary>
        /// The connection being dragged out.
        /// </summary>
        public object Connection => connection;

        #region Private Methods

        internal ConnectionDraggingEventArgs(RoutedEvent routedEvent, object source,
                object node, object connection, object connector) :
            base(routedEvent, source, node, connection, connector)
        {
        }

        #endregion Private Methods
    }

    /// <summary>
    /// Defines the event handler for the ConnectionDragging event.
    /// </summary>
    public delegate void ConnectionDraggingEventHandler(object sender, ConnectionDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a connector.
    /// </summary>
    public class ConnectionDragCompletedEventArgs : ConnectionDragEventArgs
    {
        #region Private Data Members

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        private readonly object connectorDraggedOver;

        #endregion Private Data Members

        /// <summary>
        /// The ConnectorItem or it's DataContext (when non-NULL).
        /// </summary>
        public object ConnectorDraggedOver => connectorDraggedOver;

        /// <summary>
        /// The connection that will be dragged out.
        /// </summary>
        public object Connection => connection;

        #region Private Methods

        internal ConnectionDragCompletedEventArgs(RoutedEvent routedEvent, object source, object node, object connection, object connector, object connectorDraggedOver) :
            base(routedEvent, source, node, connection, connector)
        {
            this.connectorDraggedOver = connectorDraggedOver;
        }

        #endregion Private Methods
    }

    /// <summary>
    /// Defines the event handler for the ConnectionDragCompleted event.
    /// </summary>
    public delegate void ConnectionDragCompletedEventHandler(object sender, ConnectionDragCompletedEventArgs e);
}
