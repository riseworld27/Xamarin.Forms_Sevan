using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using XOCV.Helpers;
using XOCV.Interfaces;
using XOCV.Models;
using Xamarin.Forms;
using XOCV.iOS.PlatformSpecific.Camera;

[assembly: Dependency(typeof(MediaPicker))]
namespace XOCV.iOS.PlatformSpecific.Camera
{
    public class MediaPicker : ICameraProvider
    {
        internal const string TypeImage = "public.image";

        private UIImagePickerControllerDelegate _pickerDelegate;

        private UIPopoverController _popover;

        public MediaPicker()
        {
            IsCameraAvailable = UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);

            var availableCameraMedia = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera)
                                       ?? new string[0];
            var availableLibraryMedia =
                UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) ?? new string[0];

            foreach (var type in availableCameraMedia.Concat(availableLibraryMedia))
            {
                if (type == TypeImage)
                {
                    IsPhotosSupported = true;
                }
            }
        }

        public bool IsCameraAvailable { get; private set; }

        public bool IsPhotosSupported { get; private set; }

        public EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }

        public EventHandler<MediaPickerErrorArgs> OnError { get; set; }

        public Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options)
        {
            if (!IsPhotosSupported)
            {
                throw new NotSupportedException();
            }

            return GetMediaAsync(UIImagePickerControllerSourceType.PhotoLibrary, TypeImage);
        }
        public Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options)
        {
            if (!IsPhotosSupported)
            {
                throw new NotSupportedException();
            }
            if (!IsCameraAvailable)
            {
                throw new NotSupportedException();
            }

            VerifyCameraOptions(options);

            return GetMediaAsync(UIImagePickerControllerSourceType.Camera, TypeImage, options);
        }

        private Task<MediaFile> GetMediaAsync(
            UIImagePickerControllerSourceType sourceType,
            string mediaType,
            MediaStorageOptions options = null)
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            if (window == null)
            {
                throw new InvalidOperationException("There's no current active window");
            }

            var viewController = window.RootViewController;

#if __IOS_10__
            if (viewController == null || (viewController.PresentedViewController != null && viewController.PresentedViewController.GetType() == typeof(UIAlertController)))
            {
                window =
                    UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel)
                        .FirstOrDefault(w => w.RootViewController != null);

                if (window == null)
                {
                    throw new InvalidOperationException("Could not find current view controller");
                }

                viewController = window.RootViewController;
            }
#endif 
            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            var ndelegate = new MediaPickerDelegate(viewController, sourceType, options);
            var od = Interlocked.CompareExchange(ref _pickerDelegate, ndelegate, null);
            if (od != null)
            {
                throw new InvalidOperationException("Only one operation can be active at at time");
            }

            var picker = SetupController(ndelegate, sourceType, mediaType, options);

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad
                && sourceType == UIImagePickerControllerSourceType.PhotoLibrary)
            {
                ndelegate.Popover = new UIPopoverController(picker)
                {
                    Delegate = new MediaPickerPopoverDelegate(ndelegate, picker)
                };
                ndelegate.DisplayPopover();
            }
            else
            {
                viewController.PresentViewController(picker, true, null);
            }

            return ndelegate.Task.ContinueWith(
                t =>
                {
                    if (_popover != null)
                    {
                        _popover.Dispose();
                        _popover = null;
                    }

                    Interlocked.Exchange(ref _pickerDelegate, null);
                    return t;
                }).Unwrap();
        }

        private static MediaPickerController SetupController(
            MediaPickerDelegate mpDelegate,
            UIImagePickerControllerSourceType sourceType,
            string mediaType,
            MediaStorageOptions options = null)
        {
            var picker = new MediaPickerController(mpDelegate) { MediaTypes = new[] { mediaType }, SourceType = sourceType };

            if (sourceType == UIImagePickerControllerSourceType.Camera)
            {
                if (mediaType == TypeImage && options is CameraMediaStorageOptions)
                {
                    picker.CameraDevice = GetCameraDevice(((CameraMediaStorageOptions)options).DefaultCamera);
                    picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
                }
            }

            return picker;
        }

        private static UIImagePickerControllerCameraDevice GetCameraDevice(CameraDevice device)
        {
            switch (device)
            {
                case CameraDevice.Front:
                    return UIImagePickerControllerCameraDevice.Front;
                case CameraDevice.Rear:
                    return UIImagePickerControllerCameraDevice.Rear;
                default:
                    throw new NotSupportedException();
            }
        }

        private static void VerifyOptions(MediaStorageOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            if (options.Directory != null && Path.IsPathRooted(options.Directory))
            {
                throw new ArgumentException("options.Directory must be a relative path", "options");
            }
        }

        private static void VerifyCameraOptions(CameraMediaStorageOptions options)
        {
            VerifyOptions(options);
            if (!Enum.IsDefined(typeof(CameraDevice), options.DefaultCamera))
            {
                throw new ArgumentException("options.Camera is not a member of CameraDevice");
            }
        }
    }
}