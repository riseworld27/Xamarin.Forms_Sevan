using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using XOCV.Models;

namespace XOCV.iOS.PlatformSpecific.Camera
{
    public sealed class MediaPickerController : UIImagePickerController
    {
        internal MediaPickerController(MediaPickerDelegate mpDelegate)
        {
            base.Delegate = mpDelegate;
        }

        public override NSObject Delegate
        {
            get
            {
                return base.Delegate;
            }
            set
            {
				base.Delegate = value;
            }
        }

        public Task<MediaFile> GetResultAsync()
        {
            return ((MediaPickerDelegate)Delegate).Task;
        }
    }
}