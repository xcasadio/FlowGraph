using System;
using Utils;
using System.Windows.Media;
using System.Windows;
using FlowGraphBase.Node;

namespace NetworkModel
{
    /// <summary>
    /// Defines a connection between two connectors (aka connection points) of two nodes.
    /// </summary>
    public sealed class ConnectionViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel sourceConnector = null;

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel destConnector = null;

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        private Point sourceConnectorHotspot;
        private Point destConnectorHotspot;

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        private PointCollection points = null;

        //private bool m_IsActivated = false;

        //private event EventHandler Activated;

        #endregion Internal Data Members

        /// <summary>
        /// Event fired when the connection has changed.
        /// </summary>
        public event EventHandler<EventArgs> ConnectionChanged;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ConnectorViewModel ConnectedConnector => SourceConnector == null ? DestConnector : SourceConnector;

        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel SourceConnector
        {
            get => sourceConnector;
            set
            {
                if (sourceConnector == value)
                {
                    return;
                }

                if (sourceConnector != null)
                {
                    sourceConnector.AttachedConnections.Remove(this);
                    sourceConnector.HotspotUpdated -= new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    //sourceConnector.SourceSlot.Activated -= new EventHandler(OnSourceSlotActivated); 
                }

                sourceConnector = value;

                if (sourceConnector != null)
                {
                    sourceConnector.AttachedConnections.Add(this);
                    sourceConnector.HotspotUpdated += new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    //sourceConnector.SourceSlot.Activated += new EventHandler(OnSourceSlotActivated);
                    SourceConnectorHotspot = sourceConnector.Hotspot;
                }

                OnPropertyChanged("SourceConnector");
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel DestConnector
        {
            get => destConnector;
            set
            {
                if (destConnector == value)
                {
                    return;
                }

                if (destConnector != null)
                {
                    destConnector.AttachedConnections.Remove(this);
                    destConnector.HotspotUpdated -= new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                    //destConnector.SourceSlot.Activated -= new EventHandler(OnSourceSlotActivated); 
                }

                destConnector = value;

                if (destConnector != null)
                {
                    destConnector.AttachedConnections.Add(this);
                    destConnector.HotspotUpdated += new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                    //destConnector.SourceSlot.Activated += new EventHandler(OnSourceSlotActivated);
                    DestConnectorHotspot = destConnector.Hotspot;
                }

                OnPropertyChanged("DestConnector");
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        public Point SourceConnectorHotspot
        {
            get => sourceConnectorHotspot;
            set
            {
                if (sourceConnectorHotspot != value)
                {
                    sourceConnectorHotspot = value;
                    ComputeConnectionPoints();
                    OnPropertyChanged("SourceConnectorHotspot");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point DestConnectorHotspot
        {
            get => destConnectorHotspot;
            set
            {
                if (destConnectorHotspot != value)
                {
                    destConnectorHotspot = value;
                    ComputeConnectionPoints();
                    OnPropertyChanged("DestConnectorHotspot");
                }
            }
        }

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        public PointCollection Points
        {
            get => points;
            set
            {
                points = value;
                OnPropertyChanged("Points");
            }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Warning : there are events on collection DestConnector, SourceConnector, Points
        /// </summary>
        /// <returns></returns>
        public ConnectionViewModel Copy()
        {
            ConnectionViewModel newConn = new ConnectionViewModel();

            newConn.DestConnector = DestConnector;
            newConn.DestConnectorHotspot = DestConnectorHotspot;
            newConn.Points = Points;
            newConn.SourceConnector = SourceConnector;
            newConn.SourceConnectorHotspot = SourceConnectorHotspot;

            return newConn;
        }

        #region Private Methods

        /// <summary>
        /// Raises the 'ConnectionChanged' event.
        /// </summary>
        private void OnConnectionChanged()
        {
            if (ConnectionChanged != null)
            {
                ConnectionChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event raised when the hotspot of the source connector has been updated.
        /// </summary>
        private void sourceConnector_HotspotUpdated(object sender, EventArgs e)
        {
            SourceConnectorHotspot = SourceConnector.Hotspot;
        }

        /// <summary>
        /// Event raised when the hotspot of the dest connector has been updated.
        /// </summary>
        private void destConnector_HotspotUpdated(object sender, EventArgs e)
        {
            DestConnectorHotspot = DestConnector.Hotspot;
        }

        /// <summary>
        /// Rebuild connection points.
        /// </summary>
        private void ComputeConnectionPoints()
        {
            const double offset = 120.0;

            double srcDeltaX = offset;
            double destDeltaX = -offset;

            if (SourceConnector != null)
            {
                if (SourceConnector.Type == ConnectorType.Input
                    || SourceConnector.Type == ConnectorType.VariableInput)
                {
                    srcDeltaX = -offset;
                }
                else if (SourceConnector.Type == ConnectorType.Output
                    || SourceConnector.Type == ConnectorType.VariableOutput)
                {
                    srcDeltaX = offset;
                }
            }

            if (DestConnector != null)
            {
                if (DestConnector.Type == ConnectorType.Output
                    || DestConnector.Type == ConnectorType.VariableOutput
                    || DestConnector.Type == ConnectorType.VariableInputOutput)
                {
                    destDeltaX = offset;
                }
            }

            PointCollection computedPoints = new PointCollection();
            computedPoints.Add(SourceConnectorHotspot);
            computedPoints.Add(new Point(SourceConnectorHotspot.X + srcDeltaX, SourceConnectorHotspot.Y));
            computedPoints.Add(new Point(DestConnectorHotspot.X + destDeltaX, DestConnectorHotspot.Y));
            computedPoints.Add(DestConnectorHotspot);
            computedPoints.Freeze();

            Points = computedPoints;
        }

        #endregion // Private Methods

        #endregion // Methods
    }
}
