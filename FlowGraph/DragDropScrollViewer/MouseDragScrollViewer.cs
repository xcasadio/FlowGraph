using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MouseDragScrollViewer
{
    /// <summary>
    /// ScrollViewer which can be scrolled by the mouse when dragging
    /// </summary>
    public class MouseDragScrollViewer : ScrollViewer
    {
        //         public static readonly DependencyProperty IsMouseDraggingProperty =
//                 DependencyProperty.Register("IsMouseDragging", typeof(bool), typeof(MouseDragScrollViewer),
//                                             new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty DragIntervalProperty =
                DependencyProperty.Register("DragInterval", typeof(double), typeof(MouseDragScrollViewer),
                                            new FrameworkPropertyMetadata(10.0));

        public static readonly DependencyProperty DragAccelerationProperty =
                DependencyProperty.Register("DragAcceleration", typeof(double), typeof(MouseDragScrollViewer),
                                            new FrameworkPropertyMetadata(0.0005));

        public static readonly DependencyProperty DragMaxVelocityProperty =
                DependencyProperty.Register("DragMaxVelocity", typeof(double), typeof(MouseDragScrollViewer),
                                            new FrameworkPropertyMetadata(2.0));

        public static readonly DependencyProperty DragInitialVelocityProperty =
                DependencyProperty.Register("DragInitialVelocity", typeof(double), typeof(MouseDragScrollViewer),
                                            new FrameworkPropertyMetadata(0.05));

        public static readonly DependencyProperty DragMarginProperty =
                DependencyProperty.Register("DragMargin", typeof(double), typeof(MouseDragScrollViewer),
                                            new FrameworkPropertyMetadata(40.0));

        /// <summary>
        /// Set to 'true' to enable the scrolling handled by the mouse dragging.
        /// This is set to 'false' by default.
        /// </summary>
//         public bool IsMouseDragging
//         {
//             get
//             {
//                 return (bool)GetValue(IsMouseDraggingProperty);
//             }
//             set
//             {
//                 SetValue(IsMouseDraggingProperty, value);
//             }
//         }

        /// <summary>
        /// milliseconds
        /// </summary>
        public double DragInterval
        {
            get => (double)GetValue(DragIntervalProperty);
            set => SetValue(DragIntervalProperty, value);
        }

        /// <summary>
        /// pixels per millisecond²
        /// </summary>
        public double DragAcceleration
        {
            get => (double)GetValue(DragAccelerationProperty);
            set => SetValue(DragAccelerationProperty, value);
        }
        
        /// <summary>
        /// pixels per millisecond
        /// </summary>
        public double DragMaxVelocity
        {
            get => (double)GetValue(DragMaxVelocityProperty);
            set => SetValue(DragMaxVelocityProperty, value);
        }

        /// <summary>
        /// pixels per millisecond
        /// </summary>
        public double DragInitialVelocity
        {
            get => (double)GetValue(DragInitialVelocityProperty);
            set => SetValue(DragInitialVelocityProperty, value);
        }

        /// <summary>
        /// pixels per millisecond
        /// </summary>
        public double DragMargin
        {
            get => (double)GetValue(DragMarginProperty);
            set => SetValue(DragMarginProperty, value);
        }

        private DispatcherTimer _dragScrollTimer;
        private double _dragVelocity;

        public delegate void DragDelegate(double offset);

        public DragDelegate DragHorizontalDelegate { get; set; }
        public DragDelegate DragVerticalDelegate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseDown()
        {
            Point p = MouseUtilities.GetMousePosition(this);
            if ((p.Y < DragMargin) || (p.Y > RenderSize.Height - DragMargin)
                || (p.X < DragMargin) || (p.X > RenderSize.Width - DragMargin))
            {
                if (_dragScrollTimer == null)
                {
                    _dragVelocity = DragInitialVelocity;
                    _dragScrollTimer = new DispatcherTimer();
                    _dragScrollTimer.Tick += TickDragScroll;
                    _dragScrollTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)DragInterval);
                    _dragScrollTimer.Start();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void DoMouseUp()
        {
            CancelDrag();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TickDragScroll(object sender, EventArgs e)
        {
            bool isDone = true;

            if (IsLoaded)
            {
                Rect bounds = new Rect(RenderSize);
                Point p = MouseUtilities.GetMousePosition(this);
                if (bounds.Contains(p))
                {
                    int dir = 0;

                    if (p.X < DragMargin)
                    {
                        dir |= (int)DragDirection.Left;
                        isDone = false;
                    }
                    else if (p.X > RenderSize.Width - DragMargin)
                    {
                        dir |= (int)DragDirection.Right;
                        isDone = false;
                    }

                    if (p.Y < DragMargin)
                    {
                        dir |= (int)DragDirection.Up;
                        isDone = false;
                    }
                    else if (p.Y > RenderSize.Height - DragMargin)
                    {
                        dir |= (int)DragDirection.Down;
                        isDone = false;
                    }

                    if (dir != 0)
                    {
                        DragScroll(dir);
                    }
                }
            }

            if (isDone)
            {
                CancelDrag();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CancelDrag()
        {
            if (_dragScrollTimer != null)
            {
                _dragScrollTimer.Tick -= TickDragScroll;
                _dragScrollTimer.Stop();
                _dragScrollTimer = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        private enum DragDirection
        {
            Down    = 1,
            Up      = 2,
            Left    = 4,
            Right   = 8
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        private void DragScroll(int direction_)
        {
            double hOffset = 0.0;
            double vOffset = 0.0;

            if ((direction_ & (int)DragDirection.Left) > 0)
            {
                hOffset = -_dragVelocity * DragInterval;
            }
            else if ((direction_ & (int)DragDirection.Right) > 0)
            {
                hOffset = _dragVelocity * DragInterval;
            }

            if ((direction_ & (int)DragDirection.Up) > 0)
            {
                vOffset = -_dragVelocity * DragInterval;
            }
            else if ((direction_ & (int)DragDirection.Down) > 0)
            {
                vOffset = _dragVelocity * DragInterval;
            }

            DragHorizontalDelegate?.Invoke(hOffset);
            DragVerticalDelegate?.Invoke(vOffset);

            _dragVelocity = Math.Min(DragMaxVelocity, _dragVelocity + (DragAcceleration * DragInterval));
        }
    }
}
