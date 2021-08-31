using System;
using System.Windows;
using System.Windows.Media;
using FlowGraphBase.Node;
using Utils;

namespace NetworkModel
{
    /// <summary>
    /// Defines a connection between two connectors (aka connection points) of two nodes.
    /// </summary>
    public sealed class ConnectionViewModel : AbstractModelBase
    {
        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel sourceConnector;

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel destConnector;

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        private Point sourceConnectorHotspot;
        private Point destConnectorHotspot;

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        private PointCollection points;

        //private bool _IsActivated = false;

        //private event EventHandler Activated;

        /// <summary>
        /// Event fired when the connection has changed.
        /// </summary>
        public event EventHandler<EventArgs> ConnectionChanged;

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
                    sourceConnector.HotspotUpdated -= sourceConnector_HotspotUpdated;
                    //sourceConnector.SourceSlot.Activated -= new EventHandler(OnSourceSlotActivated); 
                }

                sourceConnector = value;

                if (sourceConnector != null)
                {
                    sourceConnector.AttachedConnections.Add(this);
                    sourceConnector.HotspotUpdated += sourceConnector_HotspotUpdated;
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
                    destConnector.HotspotUpdated -= destConnector_HotspotUpdated;
                    //destConnector.SourceSlot.Activated -= new EventHandler(OnSourceSlotActivated); 
                }

                destConnector = value;

                if (destConnector != null)
                {
                    destConnector.AttachedConnections.Add(this);
                    destConnector.HotspotUpdated += destConnector_HotspotUpdated;
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

        /// <summary>
        /// Warning : there are events on collection DestConnector, SourceConnector, Points
        /// </summary>
        /// <returns></returns>
        public ConnectionViewModel Copy()
        {
            ConnectionViewModel newConn = new ConnectionViewModel
            {
                DestConnector = DestConnector,
                DestConnectorHotspot = DestConnectorHotspot,
                Points = Points,
                SourceConnector = SourceConnector,
                SourceConnectorHotspot = SourceConnectorHotspot
            };

            return newConn;
        }

        /// <summary>
        /// Raises the 'ConnectionChanged' event.
        /// </summary>
        private void OnConnectionChanged()
        {
            ConnectionChanged?.Invoke(this, EventArgs.Empty);
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
    }
}
