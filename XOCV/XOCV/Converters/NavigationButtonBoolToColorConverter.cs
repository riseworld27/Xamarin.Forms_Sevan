using System;
using Xamarin.Forms;

namespace XOCV.Converters
{
    public class NavigationButtonBoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool) value)
            {
                return Color.Gray;
            }
            return Color.Black;
            //return (bool) value ? Color.Gray : Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Color.Black : Color.Gray;
        }
    }
}