using System;
using System.Linq;
using Foundation;
using Photos;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XOCV.iOS.Renderers;
using XOCV.iOS.Services;
using XOCV.Pages;

[assembly: ExportRenderer (typeof (PollPage), typeof (PollPageRenderer))]
namespace XOCV.iOS.Renderers
{
    public class PollPageRenderer : PageRenderer
    {
        string lastImagePath = string.Empty;

        public PollPageRenderer ()
        {
            PictureService.renderer = this;
        }

        public void SavePictureToGallery (byte [] imageData)
        {
            var chartImage = new UIImage (NSData.FromArray (imageData));
            InvokeOnMainThread (() => 
            {
                chartImage.SaveToPhotosAlbum ((image, error) => 
                {
                    if (error != null)
                    {
                        Console.WriteLine (error);
                    }
                });
            });
        }

        public string GetLastImagePath ()
        {
            PHFetchOptions options = new PHFetchOptions ();
            options.SortDescriptors = new NSSortDescriptor [] { new NSSortDescriptor ("creationDate", false) };
            options.FetchLimit = 1;
            PHFetchResult fetchResult = PHAsset.FetchAssets (PHAssetMediaType.Image, options);

            var lastImageAsset = fetchResult.First () as PHAsset;

            PHImageManager manager = PHImageManager.DefaultManager;
            manager.RequestImageForAsset (lastImageAsset, PHImageManager.MaximumSize, PHImageContentMode.AspectFit, null, (result, info) => 
            {
                if (info.ObjectForKey (PHImageKeys.Error) == null)
                {
                    var lastImageUrl = info.Values [0] as NSUrl;
                    lastImagePath = lastImageUrl.FilePathUrl.AbsoluteString;
                }
            });
            return lastImagePath;
        }
    }
}