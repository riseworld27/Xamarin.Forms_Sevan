using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XOCV.Droid.PlatformSpecific.Camera;
using XOCV.Helpers;
using XOCV.Interfaces;
using XOCV.Models;
using Android.App;
using Android.Provider;
using System.Threading;

[assembly: Dependency(typeof(MediaPicker))]
namespace XOCV.Droid.PlatformSpecific.Camera
{
    public class MediaPicker : ICameraProvider
    {
        private static Context Context
        {
            get { return Android.App.Application.Context; }
        }

        private TaskCompletionSource<MediaFile> _completionSource;

        public bool IsCameraAvailable
        {
            get
            {
                var isCameraAvailable = Context.PackageManager.HasSystemFeature(PackageManager.FeatureCamera);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread)
                {
                    isCameraAvailable |= Context.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFront);
                }

                return isCameraAvailable;
            }
        }

        public bool IsPhotosSupported { get; private set; }

        private int _requestId;
        private int GetRequestId()
        {
            var id = _requestId;
            if (_requestId == int.MaxValue)
            {
                _requestId = 0;
            }
            else
            {
                _requestId++;
            }

            return id;
        }

        public EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }

        public EventHandler<MediaPickerErrorArgs> OnError { get; set; }

        public Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options)
        {
            if (!IsCameraAvailable)
            {
                throw new NotSupportedException();
            }

            options.VerifyOptions();

            return TakeMediaAsync("image/*", Intent.ActionPick, options);
        }

        public Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options)
        {
            if (!IsCameraAvailable)
            {
                throw new NotSupportedException();
            }

            options.VerifyOptions();

            return TakeMediaAsync("image/*", MediaStore.ActionImageCapture, options);
        }

        private Task<MediaFile> TakeMediaAsync(string type, string action, MediaStorageOptions options)
        {
            var id = GetRequestId();

            var ntcs = new TaskCompletionSource<MediaFile>(id);
            if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
            {
                throw new InvalidOperationException("Only one operation can be active at a time");
            }

            Context.StartActivity(CreateMediaIntent(id, type, action, options));

            EventHandler<MediaPickedEventArgs> handler = null;
            handler = (s, e) =>
            {
                var tcs = Interlocked.Exchange(ref _completionSource, null);

                MediaPickerActivity.MediaPicked -= handler;

                if (e.RequestId != id)
                {
                    return;
                }

                if (e.Error != null)
                {
                    tcs.SetException(e.Error);
                }
                else if (e.IsCanceled)
                {
                    tcs.SetCanceled();
                }
                else
                {
                    tcs.SetResult(e.Media);
                }
            };

            MediaPickerActivity.MediaPicked += handler;

            return ntcs.Task;
        }
        private Intent CreateMediaIntent(int id, string type, string action, MediaStorageOptions options, bool tasked = true)
        {
            var pickerIntent = new Intent(Context, typeof(MediaPickerActivity));
            pickerIntent.SetFlags(ActivityFlags.NewTask);
            pickerIntent.PutExtra(MediaPickerActivity.ExtraId, id);
            pickerIntent.PutExtra(MediaPickerActivity.ExtraType, type);
            pickerIntent.PutExtra(MediaPickerActivity.ExtraAction, action);
            pickerIntent.PutExtra(MediaPickerActivity.ExtraTasked, tasked);

            if (options != null)
            {
                pickerIntent.PutExtra(MediaPickerActivity.ExtraPath, options.Directory);
                pickerIntent.PutExtra(MediaStore.Images.ImageColumns.Title, options.Name);

                //var vidOptions = options as VideoMediaStorageOptions;
                //if (vidOptions != null)
                //{
                //    pickerIntent.PutExtra(MediaStore.ExtraDurationLimit, (int)vidOptions.DesiredLength.TotalSeconds);
                //    pickerIntent.PutExtra(MediaStore.ExtraVideoQuality, (int)vidOptions.Quality);
                //}
            }

            return pickerIntent;
        }
    }
}