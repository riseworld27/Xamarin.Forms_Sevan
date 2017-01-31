using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ComponentPro.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XOCV.Droid;
using XOCV.Interfaces;
using XOCV.Droid.Renderers;
using Android.Graphics;

[assembly: Xamarin.Forms.Dependency(typeof(PictureService))]
namespace XOCV.Droid
{
    public class PictureService : IPictureService
    {
        Sftp client;
        string documentsDirectory;
        public static PollPageRenderer renderer;

        public PictureService()
        {
            documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public string Base64FromFile(string name)
        {
            byte[] data = File.ReadAllBytes(name);
            string base64string = Convert.ToBase64String(data);
            return base64string;
        }

        public string GetLastImagePath()
        {
            throw new NotImplementedException();
        }

        public string GetPhotoPath(string imageName)
        {
            return System.IO.Path.Combine(documentsDirectory, imageName);
        }

        async Task IPictureService.SavePictureToDisk (ImageSource imgSrc, string fileName)
        {
            try {
                var renderer = new StreamImagesourceHandler ();
                var photo = await renderer.LoadImageAsync (imgSrc, Forms.Context);
                var documentsDirectory = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
                //var documentsDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
                string jpgFilename = System.IO.Path.Combine (documentsDirectory, fileName + ".jpg");
                using (FileStream fs = new FileStream (jpgFilename, FileMode.OpenOrCreate)) {
                    photo.Compress (Bitmap.CompressFormat.Jpeg, 100, fs);
                    fs.Close();
                }
            } catch (Exception ex) {
                string exMessageString = ex.Message;
            }

            //var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            //var pictures = dir.AbsolutePath;

            ////adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
            //string name = fileName + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            //string filePath = System.IO.Path.Combine(pictures, name);

            //try
            //{
            //    System.IO.File.WriteAllBytes(filePath, imgSrc );
            //    //mediascan adds the saved image into the gallery
            //    var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            //    mediaScanIntent.SetData(Uri.FromFile(new File(filePath)));
            //    Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
            //}
            //catch(System.Exception e)
            //{
            //    System.Console.WriteLine(e.ToString());
            //}

        }

        public string GetPictureFromDisk(string fileName)
        {
            string jpgFilename = System.IO.Path.Combine(documentsDirectory, fileName + ".jpg");
            return jpgFilename;
        }

        public int[] GetPictureSize(ImageSource imgSrc)
        {
            int[] properties = new int[3];
            // var renderer = new StreamImagesourceHandler ();
            //var photo = renderer.LoadImageAsync (imgSrc).Result;
            //properties [0] = Convert.ToInt32 (photo.Size.Width);
            //properties [1] = Convert.ToInt32 (photo.Size.Height);
            //if (photo.Orientation == UIImageOrientation.Up || photo.Orientation == UIImageOrientation.Down) {
            //    EditImageRenderer.CurrentOrientation = UIInterfaceOrientationMask.Portrait;
            //    properties [2] = 0;
            //    //AppDelegate.orientation = UIInterfaceOrientationMask.Portrait;
            //} else if (photo.Orientation == UIImageOrientation.Left || photo.Orientation == UIImageOrientation.Right) {
            //    EditImageRenderer.CurrentOrientation = UIInterfaceOrientationMask.LandscapeLeft;
            //    properties [2] = 1;
            //    //AppDelegate.orientation = UIInterfaceOrientationMask.LandscapeLeft;
            //} else {
            //    EditImageRenderer.CurrentOrientation = UIInterfaceOrientationMask.Portrait;
            //    properties [2] = 0;
            //    //AppDelegate.orientation = UIInterfaceOrientationMask.AllButUpsideDown;
            //}
            return properties;
        }

        public byte[] ImagePathToBinary(string imageName)
        {
            FileStream fileStream = new FileStream(imageName, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        public Task<string> SaveImageToDisk(string imgSrc, string fileName)
        {
            throw new NotImplementedException();
        }

        public byte[] ImageToBinary(string imageName)
        {
            string jpgFilename = System.IO.Path.Combine(documentsDirectory, imageName);
            return ImagePathToBinary(jpgFilename);
        }

        public void SavePicture(byte[] sourceArray, int width, int height, string fileName)
        {
            throw new NotImplementedException();
        }

        public void SavePictureToGallery(string imageName)
        {
            byte[] imageData = System.IO.File.ReadAllBytes(imageName);
            renderer.SavePictureToGallery(imageData);
        }

        public async Task<bool> SyncImages(List<string> imageNames)
        {
            if (imageNames.Count == 0)
                return false;
            bool success = true;

            // Connect to the SFTP server.
            client.Connect("50.112.202.60", 22);

            // Authenticate.
            client.Authenticate("sevan-img.pilgrimconsulting.com", "8A$sevan");

            foreach (string name in imageNames)
            {
                try
                {
                    string storNum = name.Split('_')[0];
                    var dirpath = string.Format("/home/sevan-img.pilgrimconsulting.com/captures/PSN/{0}", storNum);
                    if (!client.DirectoryExists(dirpath))
                    {
                        client.CreateDirectory(dirpath);
                    }

                    string jpgFilename = System.IO.Path.Combine(documentsDirectory, name);
                    var fileStream = new FileStream(jpgFilename, FileMode.Open, FileAccess.Read);

                    var r = client.UploadFile(fileStream, dirpath + "/" + name);
                    fileStream.Flush();
                    fileStream.Close();

                    //client.UploadFile (name + ".jpg",);
                    if (r == 0)
                    {
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    var exMes = ex.Message;
                }
            }
            client.Disconnect();
            return success;
        }
    }
}