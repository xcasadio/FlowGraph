using System;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Collections;
using System.Windows.Input;

//
// This code based on code available here:
//
//  http://www.codeproject.com/KB/WPF/WPFJoshSmith.aspx
//
namespace Utils
{
    //
    // This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
    //
    public class FrameworkElementAdorner : Adorner
    {
        //
        // The framework element that is the adorner. 
        //
        private FrameworkElement child = null;

        //
        // Placement of the child.
        //
        private AdornerPlacement horizontalAdornerPlacement = AdornerPlacement.Inside;
        private AdornerPlacement verticalAdornerPlacement = AdornerPlacement.Inside;

        //
        // Offset of the child.
        //
        private double offsetX = 0.0;
        private double offsetY = 0.0;

        //
        // Position of the child (when not set to NaN).
        //
        private double positionX = Double.NaN;
        private double positionY = Double.NaN;

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
            : base(adornedElement)
        {
            if (adornedElement == null)
            {
                throw new ArgumentNullException("adornedElement");
            }

            if (adornerChildElement == null)
            {
                throw new ArgumentNullException("adornerChildElement");
            }

            child = adornerChildElement;

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                       AdornerPlacement horizontalAdornerPlacement, AdornerPlacement verticalAdornerPlacement,
                                       double offsetX, double offsetY)
            : base(adornedElement)
        {
            if (adornedElement == null)
            {
                throw new ArgumentNullException("adornedElement");
            }

            if (adornerChildElement == null)
            {
                throw new ArgumentNullException("adornerChildElement");
            }

            child = adornerChildElement;
            this.horizontalAdornerPlacement = horizontalAdornerPlacement;
            this.verticalAdornerPlacement = verticalAdornerPlacement;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            adornedElement.SizeChanged += new SizeChangedEventHandler(adornedElement_SizeChanged);

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        private void adornedElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateMeasure();
        }

        //
        // The framework element that is the adorner. 
        //
        public FrameworkElement Child => child;

        //
        // Position of the child (when not set to NaN).
        //
        public double PositionX
        {
            get => positionX;
            set => positionX = value;
        }

        public double PositionY
        {
            get => positionY;
            set => positionY = value;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            child.Measure(constraint);
            return child.DesiredSize;
        }

        /// <summary>
        /// Determine the X coordinate of the child.
        /// </summary>
        private double DetermineX()
        {
            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                {
                    if (horizontalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        double adornerWidth = child.DesiredSize.Width;
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.X - adornerWidth) + offsetX;
                    }

                    if (horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -child.DesiredSize.Width + offsetX;
                    }
                    return offsetX;
                }
                case HorizontalAlignment.Right:
                {
                    if (horizontalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return position.X + offsetX;
                    }

                    if (horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        double adornedWidth = AdornedElement.ActualWidth;
                        return adornedWidth + offsetX;
                    }
                    else
                    {
                        double adornerWidth = child.DesiredSize.Width;
                        double adornedWidth = AdornedElement.ActualWidth;
                        double x = adornedWidth - adornerWidth;
                        return x + offsetX;
                    }
                }
                case HorizontalAlignment.Center:
                {
                    double adornerWidth = child.DesiredSize.Width;

                    if (horizontalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.X - (adornerWidth / 2)) + offsetX;
                    }

                    double adornedWidth = AdornedElement.ActualWidth;
                    double x = (adornedWidth / 2) - (adornerWidth / 2);
                    return x + offsetX;
                }
                case HorizontalAlignment.Stretch:
                {
                    return 0.0;
                }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the Y coordinate of the child.
        /// </summary>
        private double DetermineY()
        {
            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                {
                    if (verticalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        double adornerWidth = child.DesiredSize.Width;
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.Y - adornerWidth) + offsetY;
                    }

                    if (verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -child.DesiredSize.Height + offsetY;
                    }
                    return offsetY;
                }
                case VerticalAlignment.Bottom:
                {
                    if (verticalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return position.Y + offsetY;
                    }

                    if (verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        double adornedHeight = AdornedElement.ActualHeight;
                        return adornedHeight + offsetY;
                    }
                    else
                    {
                        double adornerHeight = child.DesiredSize.Height;
                        double adornedHeight = AdornedElement.ActualHeight;
                        double x = adornedHeight - adornerHeight;
                        return x + offsetY;
                    }
                }
                case VerticalAlignment.Center:
                {
                    double adornerHeight = child.DesiredSize.Height;

                    if (verticalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.Y - (adornerHeight/2)) + offsetY;
                    }

                    double adornedHeight = AdornedElement.ActualHeight;
                    double y = (adornedHeight / 2) - (adornerHeight / 2);
                    return y + offsetY;
                }
                case VerticalAlignment.Stretch:
                {
                    return 0.0;
                }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the width of the child.
        /// </summary>
        private double DetermineWidth()
        {
            if (!Double.IsNaN(PositionX))
            {
                return child.DesiredSize.Width;
            }

            switch (child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                {
                    return child.DesiredSize.Width;
                }
                case HorizontalAlignment.Right:
                {
                    return child.DesiredSize.Width;
                }
                case HorizontalAlignment.Center:
                {
                    return child.DesiredSize.Width;
                }
                case HorizontalAlignment.Stretch:
                {
                    return AdornedElement.ActualWidth;
                }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the height of the child.
        /// </summary>
        private double DetermineHeight()
        {
            if (!Double.IsNaN(PositionY))
            {
                return child.DesiredSize.Height;
            }

            switch (child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                {
                    return child.DesiredSize.Height;
                }
                case VerticalAlignment.Bottom:
                {
                    return child.DesiredSize.Height;
                }
                case VerticalAlignment.Center:
                {
                    return child.DesiredSize.Height; 
                }
                case VerticalAlignment.Stretch:
                {
                    return AdornedElement.ActualHeight;
                }
            }

            return 0.0;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = PositionX;
            if (Double.IsNaN(x))
            {
                x = DetermineX();
            }
            double y = PositionY;
            if (Double.IsNaN(y))
            {
                y = DetermineY();
            }
            double adornerWidth = DetermineWidth();
            double adornerHeight = DetermineHeight();
            child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            return finalSize;
        }

        protected override Int32 VisualChildrenCount => 1;

        protected override Visual GetVisualChild(Int32 index)
        {
            return child;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(child);
                return (IEnumerator)list.GetEnumerator();
            }
        }

        /// <summary>
        /// Disconnect the child element from the visual tree so that it may be reused later.
        /// </summary>
        public void DisconnectChild()
        {
            RemoveLogicalChild(child);
            RemoveVisualChild(child);
        }

        /// <summary>
        /// Override AdornedElement from base class for less type-checking.
        /// </summary>
        public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;
    }
}
