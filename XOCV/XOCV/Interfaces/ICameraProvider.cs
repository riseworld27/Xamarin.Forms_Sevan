using System;
using System.Threading.Tasks;
using XOCV.Helpers;
using XOCV.Models;

namespace XOCV.Interfaces
{
    public interface ICameraProvider
    {
        bool IsCameraAvailable { get; }
        bool IsPhotosSupported { get; }
        Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options);
        Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options);
        EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }
        EventHandler<MediaPickerErrorArgs> OnError { get; set; }
    }

    public class MediaPickerArgs : EventArgs
    {
        public MediaPickerArgs(MediaFile mf)
        {
            MediaFile = mf;
        }

        public MediaFile MediaFile { get; private set; }
    }

    public class MediaPickerErrorArgs : EventArgs
    {
        public MediaPickerErrorArgs(Exception ex)
        {
            Error = ex;
        }

        public Exception Error { get; private set; }
    }
}