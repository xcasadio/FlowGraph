using System.Windows;
using System.Windows.Media;
using FlowGraph.Node;
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
        private ConnectorViewModel _sourceConnector;

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel _destConnector;

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        private Point _sourceConnectorHotspot;
        private Point _destConnectorHotspot;

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        private PointCollection _points;

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
            get => _sourceConnector;
            set
            {
                if (_sourceConnector == value)
                {
                    return;
                }

                if (_sourceConnector != null)
                {
                    _sourceConnector.AttachedConnections.Remove(this);
                    _sourceConnector.HotspotUpdated -= sourceConnector_HotspotUpdated;
                    //sourceConnector.SourceSlot.Activated -= new EventHandler(OnSourceSlotActivated); 
                }

                _sourceConnector = value;

                if (_sourceConnector != null)
                {
                    _sourceConnector.AttachedConnections.Add(this);
                    _sourceConnector.HotspotUpdated += sourceConnector_HotspotUpdated;
                    //sourceConnector.SourceSlot.Activated += new EventHandler(OnSourceSlotActivated);
                    SourceConnectorHotspot = _sourceConnector.Hotspot;
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
            get => _destConnector;
            set
            {
                if (_destConnector == value)
                {
                    return;
                }

                if (_destConnector != null)
                {
                    _destConnector.AttachedConnections.Remove(this);
                    _destConnector.HotspotUpdated -= destConnector_HotspotUpdated;
                    //destConnector.SourceSlot.Activated -= new EventHandler(OnSourceSlotActivated); 
                }

                _destConnector = value;

                if (_destConnector != null)
                {
                    _destConnector.AttachedConnections.Add(this);
                    _destConnector.HotspotUpdated += destConnector_HotspotUpdated;
                    //destConnector.SourceSlot.Activated += new EventHandler(OnSourceSlotActivated);
                    DestConnectorHotspot = _destConnector.Hotspot;
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
            get => _sourceConnectorHotspot;
            set
            {
                if (_sourceConnectorHotspot != value)
                {
                    _sourceConnectorHotspot = value;
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
            get => _destConnectorHotspot;
            set
            {
                if (_destConnectorHotspot != value)
                {
                    _destConnectorHotspot = value;
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
            get => _points;
            set
            {
                _points = value;
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
