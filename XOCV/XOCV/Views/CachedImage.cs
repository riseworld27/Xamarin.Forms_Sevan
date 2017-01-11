using System.Windows.Input;
using Xamarin.Forms;

namespace XOCV.Views
{
    public class CachedImage : View
    {
		public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create("ImageUrl", typeof(string), typeof(CachedImage), null, BindingMode.OneWay);

		public string ImageUrl
		{
			get { return (string)GetValue(ImageUrlProperty); }
			set { SetValue(ImageUrlProperty, value); }
		}
    }
}