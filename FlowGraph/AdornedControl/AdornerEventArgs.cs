using System.Windows;

namespace AdornedControl
{
    public class AdornerEventArgs : RoutedEventArgs
    {
        public AdornerEventArgs(RoutedEvent routedEvent, object source, FrameworkElement adorner) :
            base(routedEvent, source)
        {
            this.Adorner = adorner;
        }

        public FrameworkElement Adorner { get; } = null;
    }

    public delegate void AdornerEventHandler(object sender, AdornerEventArgs e);
}
