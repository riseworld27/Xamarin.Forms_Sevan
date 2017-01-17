using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ComponentPro.Net;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XOCV.Interfaces;
using XOCV.iOS.Renderers;
using XOCV.iOS.Services;

[assembly: Dependency(typeof (PictureService))]
namespace XOCV.iOS.Services
{
    public class PictureService : IPictureService
    {
		//Todo: set proper value
		public static int RESIZE_LIMIT = 500;
		public static int RESIZE_RATIO = 5;
			
        Sftp client;
        string documentsDirectory;

        public static PollPageRenderer renderer;
		public static EditImageRenderer EditImageRendererInstance;
		public static SketchImageRenderer SketchImageRendererInstance;
        public PictureService ()
        {
            client = new Sftp ();
            documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

		public string GetPhotoPath(string imageName)
		{
			return Path.Combine(documentsDirectory, imageName);
		}

        public void SavePictureToGallery (string imageName)
        {
			byte[] imageData = System.IO.File.ReadAllBytes(imageName);
            renderer.SavePictureToGallery (imageData);
        }

        string IPictureService.GetLastImagePath ()
        {
            var result = renderer.GetLastImagePath ();
            return result;
        }

        public byte [] ImageToBinary (string imageName)
        {
            string jpgFilename = Path.Combine (documentsDirectory, imageName);
            FileStream fileStream = new FileStream (jpgFilename, FileMode.Open, FileAccess.Read);
            byte [] buffer = new byte [fileStream.Length];
            fileStream.Read (buffer, 0, (int)fileStream.Length);
            fileStream.Close ();
            return buffer;
        }

		public byte[] ImagePathToBinary(string imageName)
		{
			FileStream fileStream = new FileStream(imageName, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, (int)fileStream.Length);
			fileStream.Close();
			return buffer;
		}

        public async Task SavePictureToDisk (ImageSource imgSrc, string fileName)
        {
            var renderer = new StreamImagesourceHandler ();
            var photo = await renderer.LoadImageAsync (imgSrc);

			if (photo.Size.Width > RESIZE_LIMIT || photo.Size.Height > RESIZE_LIMIT)
			{
				var smallImage = GetSmallImage(photo);
				photo.Dispose();
				photo = smallImage;
			}
            string jpgFilename = Path.Combine (documentsDirectory, fileName + ".jpg");
            NSData imgData = photo.AsJPEG ();

            NSError err = null;
            if (imgData.Save (jpgFilename, false, out err))
            {
                Console.WriteLine ("saved as " + jpgFilename);
            }
            else
            {
                Console.WriteLine ("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
            }

			photo.Dispose();

			imgData.Dispose();
        }

		public async Task<string> SaveImageToDisk(string imgSrc, string fileName)
		{
			//var renderer = new ImageLoaderSourceHandler();
			UIImage photo = null;
			using (var url = new NSUrl(imgSrc))
			{
				using (var data = NSData.FromUrl(url))
				{
					photo = UIImage.LoadFromData(data);
				}
			}

			if (photo.Size.Width > RESIZE_LIMIT || photo.Size.Height > RESIZE_LIMIT)
			{
				var smallImage = GetSmallImage(photo);
				photo.Dispose();
				photo = smallImage;
			}
			string jpgFilename = Path.Combine(documentsDirectory, fileName);
			NSData imgData = photo.AsJPEG();

			NSError err = null;
			if (imgData.Save(jpgFilename, false, out err))
			{
				Console.WriteLine("saved as " + jpgFilename);
			}
			else
			{
				Console.WriteLine("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
			}

			photo.Dispose();

			imgData.Dispose();

			return jpgFilename;
		}

		public static void SavePictureToDisk(UIImage imgSrc, string fileName)
		{
			var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			NSData imgData = imgSrc.AsJPEG();
			NSError err = null;
			if (imgData.Save(fileName, false, out err))
			{
				Console.WriteLine("saved as " + fileName);
			}
			else
			{
				Console.WriteLine("NOT saved as " + fileName + " because" + err.LocalizedDescription);
			}
		}

		int[] IPictureService.GetPictureSize(ImageSource imgSrc)
		{
			int[] properties = new int[3];
			var renderer = new StreamImagesourceHandler();
			var photo = renderer.LoadImageAsync(imgSrc).Result;
			properties[0] = Convert.ToInt32(photo.Size.Width);
			properties[1] = Convert.ToInt32(photo.Size.Height);
			if (photo.Orientation == UIImageOrientation.Up || photo.Orientation == UIImageOrientation.Down)
			{
				EditImageRenderer.CurrentOrientation = UIInterfaceOrientationMask.Portrait;
				properties[2] = 0;
				//AppDelegate.orientation = UIInterfaceOrientationMask.Portrait;
			}
			else if (photo.Orientation == UIImageOrientation.Left || photo.Orientation == UIImageOrientation.Right)
			{
				EditImageRenderer.CurrentOrientation = UIInterfaceOrientationMask.LandscapeLeft;
				properties[2] = 1;
				//AppDelegate.orientation = UIInterfaceOrientationMask.LandscapeLeft;
			}
			else 
			{
				EditImageRenderer.CurrentOrientation = UIInterfaceOrientationMask.Portrait; 
				properties[2] = 0;
				//AppDelegate.orientation = UIInterfaceOrientationMask.AllButUpsideDown;
			}
			return properties;
		}

        string IPictureService.GetPictureFromDisk (string fileName)
        {
            string jpgFilename = Path.Combine (documentsDirectory, fileName + ".jpg");
            return jpgFilename;
        }

        string IPictureService.Base64FromFile (string name)
        {
            byte [] data = System.IO.File.ReadAllBytes (name);
            string base64string = Convert.ToBase64String (data);
            return base64string;
        }

        private UIImage GetSmallImage (UIImage source)
        {
			nfloat width = source.Size.Width / RESIZE_RATIO;
			nfloat height = source.Size.Height / RESIZE_RATIO;
            UIGraphics.BeginImageContext (new SizeF ((int)width, (int)height));
            source.Draw (new RectangleF (0, 0, (int)width, (int)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext ();
            UIGraphics.EndImageContext (); return resultImage;
        }

		public static byte[] ImageToByteArray(UIImage image)
		{

			if (image == null)
			{
				return null;
			}
			NSData data = null;

			try
			{
				data = image.AsPNG();
				return data.ToArray();
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				if (image != null)
				{
					image.Dispose();
					image = null;
				}
				if (data != null)
				{
					data.Dispose();
					data = null;
				}
			}
		}

        public async Task<bool> SyncImages (List<string> imageNames)
        {
            if (imageNames.Count == 0)
                return false;
            bool success = true;

            // Connect to the SFTP server.
            client.Connect ("50.112.202.60", 22);

            // Authenticate.
            client.Authenticate ("sevan-img.pilgrimconsulting.com", "8A$sevan");

            foreach (string name in imageNames)
            {
                try
                {
                    string storNum = name.Split ('_') [0];
                    var dirpath = string.Format ("/home/sevan-img.pilgrimconsulting.com/captures/PSN/{0}", storNum);
                    if (!client.DirectoryExists (dirpath))
                    {
                        client.CreateDirectory (dirpath);
                    }

                    string jpgFilename = Path.Combine (documentsDirectory, name);
                    var fileStream = new FileStream (jpgFilename, FileMode.Open, FileAccess.Read);

                    var r = client.UploadFile (fileStream, dirpath + "/" + name);
                    fileStream.Flush ();
                    fileStream.Close ();

                    //client.UploadFile (name + ".jpg",);
                    if (r == 0) {
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    var exMes = ex.Message;
                }
            }
            client.Disconnect ();
            return success;
        }

		void IPictureService.SavePicture(byte[] sourceArray, int width, int height, string fileName)
		{
			EditImageRendererInstance.ReSavePicture(sourceArray, width, height, fileName);
		}
    }
}