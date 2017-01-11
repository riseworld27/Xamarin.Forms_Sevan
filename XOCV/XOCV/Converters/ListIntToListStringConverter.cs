using System;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace XOCV.Converters
{
    public class ListIntToListStringConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourceList = value as ObservableCollection<int>;
            if (sourceList == null) return new ObservableCollection<string> ();
            ObservableCollection<string> destinationList = new ObservableCollection<string> ();
            foreach (int item in sourceList) {
                destinationList.Add (item.ToString ());
            }
            return destinationList;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sourceList = value as List<string>;
            if (sourceList == null) return new ObservableCollection<int> ();
            ObservableCollection<int> destinationList = new ObservableCollection<int> ();
            foreach (string item in sourceList) {
                destinationList.Add (int.Parse (item));
            }
            return destinationList;
        }
    }
}