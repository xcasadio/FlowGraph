using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using NetworkModel;

namespace FlowGraphUI
{
    /// <summary>
    /// Defines an arrow that has multiple points.
    /// </summary>
    public class CurvedArrow : Shape
    {
        public static readonly DependencyProperty ArrowHeadLengthProperty =
            DependencyProperty.Register("ArrowHeadLength", typeof(double), typeof(CurvedArrow),
                new FrameworkPropertyMetadata(8.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowHeadWidthProperty =
            DependencyProperty.Register("ArrowHeadWidth", typeof(double), typeof(CurvedArrow),
                new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(CurvedArrow),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent ActivatedEvent = EventManager.RegisterRoutedEvent(
            "Activated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CurvedArrow));

        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler Activated
        {
            add => AddHandler(ActivatedEvent, value);
            remove => RemoveHandler(ActivatedEvent, value);
        }

        //         RoutedEventArgs newEventArgs = new RoutedEventArgs(MyButtonSimple.TapEvent);
        //             RaiseEvent(newEventArgs);

        /// <summary>
        /// The angle (in degrees) of the arrow head.
        /// </summary>
        public double ArrowHeadLength
        {
            get => (double)GetValue(ArrowHeadLengthProperty);
            set => SetValue(ArrowHeadLengthProperty, value);
        }

        /// <summary>
        /// The width of the arrow head.
        /// </summary>
        public double ArrowHeadWidth
        {
            get => (double)GetValue(ArrowHeadWidthProperty);
            set => SetValue(ArrowHeadWidthProperty, value);
        }

        /// <summary>
        /// The intermediate points that make up the line between the start and the end.
        /// </summary>
        public PointCollection Points
        {
            get => (PointCollection)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public CurvedArrow()
        {
            DataContextChanged += OnDataContextChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ConnectionViewModel value)
            {
                if (value.SourceConnector != null)
                {
                    value.SourceConnector.SourceSlot.Activated -= OnNodeSlotActivated;
                }
            }

            if (e.NewValue is ConnectionViewModel model)
            {
                if (model.SourceConnector != null)
                {
                    model.SourceConnector.SourceSlot.Activated -= OnNodeSlotActivated;
                    model.SourceConnector.SourceSlot.Activated += OnNodeSlotActivated;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNodeSlotActivated(object sender, EventArgs e)
        {
            if (Dispatcher.CheckAccess())
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(ActivatedEvent);
                RaiseEvent(newEventArgs);
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => OnNodeSlotActivated(sender, e)));
            }
        }

        /// <summary>
        /// Return the shape's geometry.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (Points == null || Points.Count < 2)
                {
                    return new GeometryGroup();
                }

                //
                // Geometry has not yet been generated.
                // Generate geometry and cache it.
                //
                Geometry geometry = GenerateGeometry();

                GeometryGroup group = new GeometryGroup();
                group.Children.Add(geometry);

                //GenerateArrowHeadGeometry(group);

                //
                // Return cached geometry.
                //
                return group;
            }
        }

        //         private void GenerateArrowHeadGeometry(GeometryGroup geometryGroup)
        //         {
        //             Point startPoint = Points[0];
        // 
        //             Point penultimatePoint = Points[Points.Count - 2];
        //             Point arrowHeadTip = Points[Points.Count - 1];
        //             Vector startDir = arrowHeadTip - penultimatePoint;
        //             startDir.Normalize();
        //             Point basePoint = arrowHeadTip - (startDir * ArrowHeadLength);
        //             Vector crossDir = new Vector(-startDir.Y, startDir.X);
        // 
        //             Point[] arrowHeadPoints = new Point[3];
        //             arrowHeadPoints[0] = arrowHeadTip;
        //             arrowHeadPoints[1] = basePoint - (crossDir * (ArrowHeadWidth / 2));
        //             arrowHeadPoints[2] = basePoint + (crossDir * (ArrowHeadWidth / 2));
        // 
        //             //
        //             // Build geometry for the arrow head.
        //             //
        //             PathFigure arrowHeadFig = new PathFigure();
        //             arrowHeadFig.IsClosed = true;
        //             arrowHeadFig.IsFilled = true;
        //             arrowHeadFig.StartPoint = arrowHeadPoints[0];
        //             arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[1], true));
        //             arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[2], true));
        // 
        //             PathGeometry pathGeometry = new PathGeometry();
        //             pathGeometry.Figures.Add(arrowHeadFig);
        // 
        //             geometryGroup.Children.Add(pathGeometry);
        //         }

        protected Geometry GenerateGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();

            if (Points.Count == 2 || Points.Count == 3)
            {
                // Make a straight line.
                PathFigure fig = new PathFigure
                {
                    IsClosed = false,
                    IsFilled = false,
                    StartPoint = Points[0]
                };

                for (int i = 1; i < Points.Count; ++i)
                {
                    fig.Segments.Add(new LineSegment(Points[i], true));
                }

                pathGeometry.Figures.Add(fig);
            }
            else
            {
                PointCollection adjustedPoints = new PointCollection();
                adjustedPoints.Add(Points[0]);
                for (int i = 1; i < Points.Count; ++i)
                {
                    adjustedPoints.Add(Points[i]);
                }

                if (adjustedPoints.Count == 4)
                {
                    // Make a curved line.
                    PathFigure fig = new PathFigure
                    {
                        IsClosed = false,
                        IsFilled = false,
                        StartPoint = adjustedPoints[0]
                    };
                    fig.Segments.Add(new BezierSegment(adjustedPoints[1], adjustedPoints[2], adjustedPoints[3], true));

                    pathGeometry.Figures.Add(fig);
                }
                else if (adjustedPoints.Count >= 5)
                {
                    // Make a curved line.
                    PathFigure fig = new PathFigure
                    {
                        IsClosed = false,
                        IsFilled = false,
                        StartPoint = adjustedPoints[0]
                    };

                    adjustedPoints.RemoveAt(0);

                    while (adjustedPoints.Count > 3)
                    {
                        Point generatedPoint = adjustedPoints[1] + ((adjustedPoints[2] - adjustedPoints[1]) / 2);

                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[1], generatedPoint, true));

                        adjustedPoints.RemoveAt(0);
                        adjustedPoints.RemoveAt(0);
                    }

                    if (adjustedPoints.Count == 2)
                    {
                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[0], adjustedPoints[1], true));
                    }
                    else
                    {
                        Trace.Assert(adjustedPoints.Count == 2);

                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[1], adjustedPoints[2], true));
                    }

                    pathGeometry.Figures.Add(fig);
                }
            }

            return pathGeometry;
        }
    }
}
