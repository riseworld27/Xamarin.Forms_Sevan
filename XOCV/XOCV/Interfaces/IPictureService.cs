using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XOCV.Interfaces
{
    public interface IPictureService
    {
		void SavePictureToGallery(string imageName);
        string GetLastImagePath ();
        byte [] ImageToBinary (string imagePath);
        Task SavePictureToDisk (ImageSource imgSrc, string fileName);
        string GetPictureFromDisk (string fileName);
        string Base64FromFile (string name);
        Task<bool> SyncImages (List<string> imageNames);
		int[] GetPictureSize(ImageSource imgSrc);
		void SavePicture(byte[] sourceArray, int width, int height, string fileName);
		string GetPhotoPath(string imageName);
		byte[] ImagePathToBinary(string imageName);
		Task<string> SaveImageToDisk(string imgSrc, string fileName);
    }
}