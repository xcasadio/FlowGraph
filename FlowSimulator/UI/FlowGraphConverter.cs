using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using System.Windows;

namespace FlowSimulator.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(VariableTypeInspector.GetColorFromType(value as Type));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TypeToLinearGradientConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color varColor = VariableTypeInspector.GetColorFromType(value as Type);
            varColor = Color.Multiply(varColor, 0.4f);
            varColor.A = 153;

            LinearGradientBrush linearBrush = new LinearGradientBrush();
            linearBrush.StartPoint = new Point(0, 0);
            linearBrush.EndPoint = new Point(0, 1);
            linearBrush.GradientStops.Add(new GradientStop(varColor, 0));
            linearBrush.GradientStops.Add(new GradientStop(Color.FromArgb(153, 0, 0, 0), 0.4));
            
            return linearBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
