using System;
using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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

        //
        // Placement of the child.
        //
        private readonly AdornerPlacement horizontalAdornerPlacement = AdornerPlacement.Inside;
        private readonly AdornerPlacement verticalAdornerPlacement = AdornerPlacement.Inside;

        //
        // Offset of the child.
        //
        private readonly double offsetX;
        private readonly double offsetY;

        //
        // Position of the child (when not set to NaN).
        //

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

            Child = adornerChildElement;

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

            Child = adornerChildElement;
            this.horizontalAdornerPlacement = horizontalAdornerPlacement;
            this.verticalAdornerPlacement = verticalAdornerPlacement;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            adornedElement.SizeChanged += adornedElement_SizeChanged;

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
        public FrameworkElement Child { get; }

        //
        // Position of the child (when not set to NaN).
        //
        public double PositionX { get; set; } = Double.NaN;

        public double PositionY { get; set; } = Double.NaN;

        protected override Size MeasureOverride(Size constraint)
        {
            Child.Measure(constraint);
            return Child.DesiredSize;
        }

        /// <summary>
        /// Determine the X coordinate of the child.
        /// </summary>
        private double DetermineX()
        {
            switch (Child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                {
                    if (horizontalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        double adornerWidth = Child.DesiredSize.Width;
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.X - adornerWidth) + offsetX;
                    }

                    if (horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -Child.DesiredSize.Width + offsetX;
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
                        double adornerWidth = Child.DesiredSize.Width;
                        double adornedWidth = AdornedElement.ActualWidth;
                        double x = adornedWidth - adornerWidth;
                        return x + offsetX;
                    }
                }
                case HorizontalAlignment.Center:
                {
                    double adornerWidth = Child.DesiredSize.Width;

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
            switch (Child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                {
                    if (verticalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        double adornerWidth = Child.DesiredSize.Width;
                        Point position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.Y - adornerWidth) + offsetY;
                    }

                    if (verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -Child.DesiredSize.Height + offsetY;
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
                        double adornerHeight = Child.DesiredSize.Height;
                        double adornedHeight = AdornedElement.ActualHeight;
                        double x = adornedHeight - adornerHeight;
                        return x + offsetY;
                    }
                }
                case VerticalAlignment.Center:
                {
                    double adornerHeight = Child.DesiredSize.Height;

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
                return Child.DesiredSize.Width;
            }

            switch (Child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                {
                    return Child.DesiredSize.Width;
                }
                case HorizontalAlignment.Right:
                {
                    return Child.DesiredSize.Width;
                }
                case HorizontalAlignment.Center:
                {
                    return Child.DesiredSize.Width;
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
                return Child.DesiredSize.Height;
            }

            switch (Child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                {
                    return Child.DesiredSize.Height;
                }
                case VerticalAlignment.Bottom:
                {
                    return Child.DesiredSize.Height;
                }
                case VerticalAlignment.Center:
                {
                    return Child.DesiredSize.Height; 
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
            Child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            return finalSize;
        }

        protected override Int32 VisualChildrenCount => 1;

        protected override Visual GetVisualChild(Int32 index)
        {
            return Child;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(Child);
                return list.GetEnumerator();
            }
        }

        /// <summary>
        /// Disconnect the child element from the visual tree so that it may be reused later.
        /// </summary>
        public void DisconnectChild()
        {
            RemoveLogicalChild(Child);
            RemoveVisualChild(Child);
        }

        /// <summary>
        /// Override AdornedElement from base class for less type-checking.
        /// </summary>
        public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;
    }
}
