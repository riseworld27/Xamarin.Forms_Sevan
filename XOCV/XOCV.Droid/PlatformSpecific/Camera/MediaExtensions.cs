using System;
using System.IO;
using XOCV.Helpers;

namespace XOCV.Droid.PlatformSpecific.Camera
{
    public static class MediaExtensions
    {
        public static void VerifyOptions(this MediaStorageOptions self)
        {
            if (self == null)
            {
                throw new ArgumentNullException("self");
            }
            //if (!Enum.IsDefined (typeof(MediaFileStoreLocation), options.Location))
            //    throw new ArgumentException ("options.Location is not a member of MediaFileStoreLocation");
            //if (options.Location == MediaFileStoreLocation.Local)
            //{
            //if (String.IsNullOrWhiteSpace (options.Directory))
            //  throw new ArgumentNullException ("options", "For local storage, options.Directory must be set");
            if (Path.IsPathRooted(self.Directory))
            {
                throw new ArgumentException("options.Directory must be a relative folder", "self");
            }
            //}
        }


        public static string GetMediaFileWithPath(this MediaStorageOptions self, string rootPath)
        {
            var isPhoto = true; //!(self is VideoMediaStorageOptions);
            var name = (self != null) ? self.Name : null;
            var directory = (self != null) ? self.Directory : null;

            return MediaFileHelpers.GetMediaFileWithPath(isPhoto, rootPath, directory, name);
        }


        public static string GetUniqueMediaFileWithPath(this MediaStorageOptions self, string rootPath,
            Func<string, bool> checkExists)
        {
            var isPhoto = true; //!(self is VideoMediaStorageOptions);
            var path = self.GetMediaFileWithPath(rootPath);

            var folder = Path.GetDirectoryName(path);
            var name = Path.GetFileNameWithoutExtension(path);

            return MediaFileHelpers.GetUniqueMediaFileWithPath(isPhoto, folder, name, checkExists);
        }
    }

    public static class MediaFileHelpers
    {

        public static string GetMediaFileWithPath(bool isPhoto, string folder, string subdir, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                if (isPhoto)
                {
                    name = "IMG_" + timestamp + ".jpg";
                }
                else
                {
                    name = "VID_" + timestamp + ".mp4";
                }
            }

            var ext = Path.GetExtension(name);
            if (ext == String.Empty)
            {
                ext = ((isPhoto) ? ".jpg" : ".mp4");
            }

            name = Path.GetFileNameWithoutExtension(name);

            var newFolder = Path.Combine(folder ?? String.Empty, subdir ?? String.Empty);

            return Path.Combine(newFolder, name + ext);
        }


        public static string GetUniqueMediaFileWithPath(bool isPhoto, string folder, string name, Func<string, bool> checkExists)
        {
            var ext = Path.GetExtension(name);

            if (String.IsNullOrEmpty(ext))
            {
                ext = (isPhoto) ? ".jpg" : "mp4";
            }

            var nname = name + ext;
            var i = 1;
            while (checkExists(Path.Combine(folder, nname)))
            {
                nname = name + "_" + (i++) + ext;
            }

            return Path.Combine(folder, nname);
        }
    }
}