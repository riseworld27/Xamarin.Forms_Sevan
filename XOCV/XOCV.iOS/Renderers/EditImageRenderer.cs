using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XOCV.iOS.Services;
using XOCV.Pages;

[assembly: ExportRenderer(typeof(XOCV.Pages.PhotoSignaturePage), typeof(XOCV.iOS.Renderers.EditImageRenderer))]
namespace XOCV.iOS.Renderers
{
	public class EditImageRenderer : PageRenderer
	{
		public static UIInterfaceOrientationMask CurrentOrientation;


		public EditImageRenderer()
		{
			PictureService.EditImageRendererInstance = this;
		}

		public void ReSavePicture(byte[] sourceArray, int width, int height, string fileName)
		{
			InvokeOnMainThread(() =>
			{
				var sourceData = NSData.FromArray(sourceArray);
				var sourceImage = UIImage.LoadFromData(sourceData);
				//var sketchData = NSData.FromArray(sketchArray);
				var sketchImage = PictureService.SketchImageRendererInstance.CreateScreenshotImage();

				UIGraphics.BeginImageContext(new CGSize((nfloat)width, (nfloat)height));
				sourceImage.Draw(new CGRect(
					0, 0, (nfloat)width, (nfloat)height));
				sketchImage.Draw(new CGRect(
					0, 0, (nfloat)width, (nfloat)height));

				var destinationImage = UIGraphics.GetImageFromCurrentImageContext();

				UIGraphics.EndImageContext();
				PictureService.SavePictureToDisk(destinationImage, fileName);
			});
		}
	}
}
