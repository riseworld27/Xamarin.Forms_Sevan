using System;

namespace XOCV.Droid.PlatformSpecific.Camera
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Android.App;
    using Android.Content;
    using Android.Database;
    using Android.OS;
    using Android.Provider;

    using Environment = Android.OS.Environment;
    using Uri = Android.Net.Uri;
    using Android.Telecom;
    using XOCV.Models;
    using XOCV.Droid.Extensions;

    [Activity]
    internal class MediaPickerActivity
        : Activity
    {

        internal const string ExtraPath = "path";

        internal const string ExtraLocation = "location";

        internal const string ExtraType = "type";

        internal const string ExtraId = "id";

        internal const string ExtraAction = "action";

        internal const string ExtraTasked = "tasked";


        internal const string MediaFileExtraName = "MediaFile";

        private string _action;


        private string _description;

        private int _id;


        private bool _isPhoto;


        private Uri _path;

        private VideoQuality _quality;

        private int _seconds;

        private bool _tasked;

        private string _title;

        private string _type;

        internal static event EventHandler<MediaPickedEventArgs> MediaPicked;

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutBoolean("ran", true);
            outState.PutString(MediaStore.MediaColumns.Title, _title);
            outState.PutString(MediaStore.Images.ImageColumns.Description, _description);
            outState.PutInt(ExtraId, _id);
            outState.PutString(ExtraType, _type);
            outState.PutString(ExtraAction, _action);
            outState.PutInt(MediaStore.ExtraDurationLimit, _seconds);
            outState.PutInt(MediaStore.ExtraVideoQuality, (int)_quality);
            outState.PutBoolean(ExtraTasked, _tasked);

            if (_path != null)
                outState.PutString(ExtraPath, _path.Path);

            base.OnSaveInstanceState(outState);
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var b = (savedInstanceState ?? Intent.Extras);

            var ran = b.GetBoolean("ran", false);

            _title = b.GetString(MediaStore.MediaColumns.Title);
            _description = b.GetString(MediaStore.Images.ImageColumns.Description);

            _tasked = b.GetBoolean(ExtraTasked);
            _id = b.GetInt(ExtraId, 0);
            _type = b.GetString(ExtraType);

            if (_type == "image/*")
            {
                _isPhoto = true;
            }

            _action = b.GetString(ExtraAction);
            Intent pickIntent = null;

            try
            {
                pickIntent = new Intent(_action);
                if (_action == Intent.ActionPick)
                    pickIntent.SetType(_type);
                else
                {
                    if (!_isPhoto)
                    {
                        _seconds = b.GetInt(MediaStore.ExtraDurationLimit, 0);
                        if (_seconds != 0)
                        {
                            pickIntent.PutExtra(MediaStore.ExtraDurationLimit, _seconds);
                        }
                    }

                    _quality = (VideoQuality)b.GetInt(MediaStore.ExtraVideoQuality, (int)VideoQuality.High);
                    pickIntent.PutExtra(MediaStore.ExtraVideoQuality, GetVideoQuality(_quality));

                    if (!ran)
                    {
                        _path = GetOutputMediaFile(this, b.GetString(ExtraPath), _title, _isPhoto);

                        Touch();
                        pickIntent.PutExtra(MediaStore.ExtraOutput, _path);
                    }
                    else
                        _path = Uri.Parse(b.GetString(ExtraPath));
                }

                if (!ran)
                {
                    StartActivityForResult(pickIntent, _id);
                }
            }
            catch (Exception ex)
            {
                RaiseOnMediaPicked(new MediaPickedEventArgs(_id, ex));
            }
            finally
            {
                if (pickIntent != null)
                    pickIntent.Dispose();
            }
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (_tasked)
            {
                var future = resultCode == Result.Canceled
                    ? TaskUtils.TaskFromResult(new MediaPickedEventArgs(requestCode, true))
                    : GetMediaFileAsync(this, requestCode, _action, _isPhoto, ref _path, (data != null) ? data.Data : null);

                Finish();

                future.ContinueWith(t => RaiseOnMediaPicked(t.Result));
            }
            else
            {
                if (resultCode == Result.Canceled)
                {
                    SetResult(Result.Canceled);
                }
                else
                {
                    var resultData = new Intent();
                    resultData.PutExtra(MediaFileExtraName, (data != null) ? data.Data : null);
                    resultData.PutExtra(ExtraPath, _path);
                    resultData.PutExtra("isPhoto", _isPhoto);
                    resultData.PutExtra(ExtraAction, _action);

                    SetResult(Result.Ok, resultData);
                }

                Finish();
            }
        }

        internal static Task<MediaPickedEventArgs> GetMediaFileAsync(Context context, int requestCode, string action,
            bool isPhoto, ref Uri path, Uri data)
        {
            Task<Tuple<string, bool>> pathFuture;
            Action<bool> dispose = null;
            string originalPath = null;

            if (action != Intent.ActionPick)
            {
                originalPath = path.Path;

                // Not all camera apps respect EXTRA_OUTPUT, some will instead
                // return a content or file uri from data.
                if (data != null && data.Path != originalPath)
                {
                    originalPath = data.ToString();
                    var currentPath = path.Path;

                    pathFuture = TryMoveFileAsync(context, data, path, isPhoto).ContinueWith(t =>
                        new Tuple<string, bool>(t.Result ? currentPath : null, false));
                }
                else
                    pathFuture = TaskUtils.TaskFromResult(new Tuple<string, bool>(path.Path, false));
            }
            else if (data != null)
            {
                originalPath = data.ToString();
                path = data;
                pathFuture = GetFileForUriAsync(context, path, isPhoto);
            }
            else
            {
                pathFuture = TaskUtils.TaskFromResult<Tuple<string, bool>>(null);
            }

            return pathFuture.ContinueWith(t =>
            {
                string resultPath = t.Result.Item1;
                if (resultPath != null && File.Exists(t.Result.Item1))
                {
                    if (t.Result.Item2)
                    {
                        dispose = d => File.Delete(resultPath);
                    }

                    var mf = new MediaFile(resultPath, () => File.OpenRead(t.Result.Item1), dispose);

                    return new MediaPickedEventArgs(requestCode, false, mf);
                }
                return new MediaPickedEventArgs(requestCode, new FileNotFoundException("Media file not found", originalPath));
            });
        }


        private static Task<bool> TryMoveFileAsync(Context context, Uri url, Uri path, bool isPhoto)
        {
            string moveTo = GetLocalPath(path);
            return GetFileForUriAsync(context, url, isPhoto).ContinueWith(t =>
            {
                if (t.Result.Item1 == null)
                    return false;

                File.Delete(moveTo);
                File.Move(t.Result.Item1, moveTo);

                if (url.Scheme == "content")
                    context.ContentResolver.Delete(url, null, null);

                return true;
            }, TaskScheduler.Default);
        }


        private static int GetVideoQuality(VideoQuality videoQuality)
        {
            switch (videoQuality)
            {
                case VideoQuality.Medium:
                case VideoQuality.High:
                    return 1;

                default:
                    return 0;
            }
        }


        private static Uri GetOutputMediaFile(Context context, string subdir, string name, bool isPhoto)
        {
            subdir = subdir ?? String.Empty;

            if (String.IsNullOrWhiteSpace(name))
            {
                name = MediaFileHelpers.GetMediaFileWithPath(isPhoto, subdir, string.Empty, name);
            }

            var mediaType = (isPhoto) ? Environment.DirectoryPictures : Environment.DirectoryMovies;
            using (var mediaStorageDir = new Java.IO.File(context.GetExternalFilesDir(mediaType), subdir))
            {
                if (!mediaStorageDir.Exists())
                {
                    if (!mediaStorageDir.Mkdirs())
                        throw new IOException("Couldn't create directory, have you added the WRITE_EXTERNAL_STORAGE permission?");

                    // Ensure this media doesn't show up in gallery apps
                    using (var nomedia = new Java.IO.File(mediaStorageDir, ".nomedia"))
                        nomedia.CreateNewFile();
                }

                return Uri.FromFile(new Java.IO.File(MediaFileHelpers.GetUniqueMediaFileWithPath(isPhoto, mediaStorageDir.Path, name, File.Exists)));
            }
        }


        internal static Task<Tuple<string, bool>> GetFileForUriAsync(Context context, Uri uri, bool isPhoto)
        {
            var tcs = new TaskCompletionSource<Tuple<string, bool>>();

            if (uri.Scheme == "file")
                tcs.SetResult(new Tuple<string, bool>(new System.Uri(uri.ToString()).LocalPath, false));
            else if (uri.Scheme == "content")
            {
                Task.Factory.StartNew(() =>
                {
                    ICursor cursor = null;
                    try
                    {
                        cursor = context.ContentResolver.Query(uri, null, null, null, null);
                        if (cursor == null || !cursor.MoveToNext())
                            tcs.SetResult(new Tuple<string, bool>(null, false));
                        else
                        {
                            int column = cursor.GetColumnIndex(MediaStore.MediaColumns.Data);
                            string contentPath = null;

                            if (column != -1)
                                contentPath = cursor.GetString(column);

                            bool copied = false;

                            // If they don't follow the "rules", try to copy the file locally
                            //                          if (contentPath == null || !contentPath.StartsWith("file"))
                            //                          {
                            //                              copied = true;
                            //                              Uri outputPath = GetOutputMediaFile(context, "temp", null, isPhoto);
                            //
                            //                              try
                            //                              {
                            //                                  using (Stream input = context.ContentResolver.OpenInputStream(uri))
                            //                                  using (Stream output = File.Create(outputPath.Path))
                            //                                      input.CopyTo(output);
                            //
                            //                                  contentPath = outputPath.Path;
                            //                              }
                            //                              catch (FileNotFoundException)
                            //                              {
                            //                                  // If there's no data associated with the uri, we don't know
                            //                                  // how to open this. contentPath will be null which will trigger
                            //                                  // MediaFileNotFoundException.
                            //                              }
                            //                          }

                            tcs.SetResult(new Tuple<string, bool>(contentPath, copied));
                        }
                    }
                    finally
                    {
                        if (cursor != null)
                        {
                            cursor.Close();
                            cursor.Dispose();
                        }
                    }
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            }
            else
                tcs.SetResult(new Tuple<string, bool>(null, false));

            return tcs.Task;
        }


        private static string GetLocalPath(Uri uri)
        {
            return new System.Uri(uri.ToString()).LocalPath;
        }

        private void Touch()
        {
            if (_path.Scheme != "file")
                return;

            File.Create(GetLocalPath(_path)).Close();
        }

        private static void RaiseOnMediaPicked(MediaPickedEventArgs e)
        {
            var picked = MediaPicked;
            if (picked != null)
            {
                picked(null, e);
            }
        }

    }


    internal class MediaPickedEventArgs
        : EventArgs
    {

        public MediaPickedEventArgs(int id, Exception error)
        {
            if (error == null)
                throw new ArgumentNullException("error");

            RequestId = id;
            Error = error;
        }


        public MediaPickedEventArgs(int id, bool isCanceled, MediaFile media = null)
        {
            RequestId = id;
            IsCanceled = isCanceled;
            if (!IsCanceled && media == null)
                throw new ArgumentNullException("media");

            Media = media;
        }

        public int RequestId { get; private set; }

        public bool IsCanceled { get; private set; }

        public Exception Error { get; private set; }

        public MediaFile Media { get; private set; }

        public Task<MediaFile> ToTask()
        {
            var tcs = new TaskCompletionSource<MediaFile>();

            if (IsCanceled)
                tcs.SetCanceled();
            else if (Error != null)
                tcs.SetException(Error);
            else
                tcs.SetResult(Media);

            return tcs.Task;
        }

    }
}

