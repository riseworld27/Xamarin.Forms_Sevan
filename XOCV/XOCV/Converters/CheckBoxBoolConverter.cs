using System;
using System.Globalization;
using Xamarin.Forms;

namespace XOCV.Converters
{
    public class CheckBoxBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = (bool)value;
            if (temp == true)
            {
                return "selected.png";
            }
            return "unselected.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = (bool)value;
            if (temp)
            {
                return "unselected.png";
            }
            return "selected.png";
        }
    }
}