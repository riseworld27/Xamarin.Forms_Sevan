using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using UIKit;
using XOCV.Views;

[assembly: ExportRenderer(typeof(CachedImage), typeof(XOCV.iOS.CachedImageRenderer))]
namespace XOCV.iOS
{	
   	public class CachedImageRenderer : ViewRenderer<CachedImage, CachedImageControl>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CachedImage> e)
		{
			base.OnElementChanged(e);

			if (Element == null)
			{
				return;
			}

			if (Control == null)
			{
				SetNativeControl(new CachedImageControl(Bounds));
			}


			Control.ImageUrl = Element.ImageUrl;
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CachedImage.ImageUrlProperty.PropertyName)
			{
				Control.ImageUrl = Element.ImageUrl;
			}
		}
	}
}