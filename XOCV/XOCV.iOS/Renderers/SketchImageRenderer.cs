using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XOCV.iOS.Renderers;
using XOCV.Views;
using CoreGraphics;
using XOCV.iOS.Controls;
using System.ComponentModel;
using UIKit;
using XOCV.iOS.Services;

[assembly: ExportRenderer(typeof(SketchImage), typeof(SketchImageRenderer))]
namespace XOCV.iOS.Renderers
{
	public class SketchImageRenderer : ViewRenderer<SketchImage, DrawView>
	{
		public SketchImageRenderer()
		{
			PictureService.SketchImageRendererInstance = this;
		}
		protected override void OnElementChanged(ElementChangedEventArgs<SketchImage> e)
		{
			base.OnElementChanged(e);

			SetNativeControl(new DrawView(CGRect.Empty));

		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == SketchImage.CurrentLineColorProperty.PropertyName)
			{
				UpdateControl();
			}
		}

		private void UpdateControl()
		{
			Control.CurrentLineColor = Element.CurrentLineColor.ToUIColor();
		}

		public UIImage CreateScreenshotImage()
		{
			//UIView rootView = UIApplication.SharedApplication.KeyWindow.RootViewController.View;
			//if (rootView != null)
			//{
				var bounds = this.Bounds;
				UIGraphics.BeginImageContextWithOptions(bounds.Size, false, 0);
				this.DrawViewHierarchy(bounds, true);
				var screenshotImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
				return screenshotImage;
			//}
			//return null;
		}
	}
}
