using System.Windows;

namespace AdornedControl
{
    public class AdornerEventArgs : RoutedEventArgs
    {
        private FrameworkElement adorner = null;

        public AdornerEventArgs(RoutedEvent routedEvent, object source, FrameworkElement adorner) :
            base(routedEvent, source)
        {
            this.adorner = adorner;
        }

        public FrameworkElement Adorner => adorner;
    }

    public delegate void AdornerEventHandler(object sender, AdornerEventArgs e);
}
