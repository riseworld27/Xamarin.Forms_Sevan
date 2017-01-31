using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using XOCV.Droid.Renderers;
using XOCV.Pages;

[assembly: ExportRenderer(typeof(PollPage), typeof(PollPageRenderer))]
namespace XOCV.Droid.Renderers
{
    public class PollPageRenderer : PageRenderer
    {
        string lastImagePath = string.Empty;

        public PollPageRenderer()
        {
            PictureService.renderer = this;
        }

        internal void SavePictureToGallery(byte[] imageData)
        {
            //var chartImage = new UIImage(NSData.FromArray(imageData));
            //InvokeOnMainThread(() =>
            //{
            //    chartImage.SaveToPhotosAlbum((image, error) =>
            //    {
            //        if (error != null)
            //        {
            //            Console.WriteLine(error);
            //        }
            //    });
            //});
            throw new NotImplementedException();
        }

        public string GetLastImagePath()
        {
            //PHFetchOptions options = new PHFetchOptions();
            //options.SortDescriptors = new NSSortDescriptor[] { new NSSortDescriptor("creationDate", false) };
            //options.FetchLimit = 1;
            //PHFetchResult fetchResult = PHAsset.FetchAssets(PHAssetMediaType.Image, options);

            //var lastImageAsset = fetchResult.First() as PHAsset;

            //PHImageManager manager = PHImageManager.DefaultManager;
            //manager.RequestImageForAsset(lastImageAsset, PHImageManager.MaximumSize, PHImageContentMode.AspectFit, null, (result, info) =>
            //{
            //    if (info.ObjectForKey(PHImageKeys.Error) == null)
            //    {
            //        var lastImageUrl = info.Values[0] as NSUrl;
            //        lastImagePath = lastImageUrl.FilePathUrl.AbsoluteString;
            //    }
            //});
            //return lastImagePath;
            throw new NotImplementedException();
        }
    }
}