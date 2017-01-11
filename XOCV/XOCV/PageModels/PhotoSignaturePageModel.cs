using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Interfaces;
using XOCV.Models.ResponseModels;
using XOCV.ViewModels.Base;

namespace XOCV.PageModels
{
	[ImplementPropertyChanged]
	public class PhotoSignaturePageModel : BasePageModel
	{
		public string MediaPath { get; set; }
		public Image ImageMedia { get; set; }
		public int[] ImageSize { get; set; }
		public IPictureService PictureService { get; set; }
		public ICommand SavePictureCommand => new Command(() => SavePictureCommandExecute());
		public ImageSource SketchImageSource { get; set; }

		public PhotoSignaturePageModel()
		{
			
		}

		protected override void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing(sender, e);
			MessagingCenter.Send<PhotoSignaturePageModel>(this, "OnPhotoEdit");
		}

		protected override void ViewIsDisappearing(object sender, EventArgs e)
		{
			base.ViewIsDisappearing(sender, e);
			MessagingCenter.Send<PhotoSignaturePageModel>(this, "OnPhotoEditFinishing");
		}

		public override void Init(object initData)
		{
			PictureService = DependencyService.Get<IPictureService>();
			base.Init(initData);
			MediaPath = (string)initData;
			var byteArray = DependencyService.Get<IPictureService>().ImagePathToBinary(MediaPath);
			var streamSource = new StreamImageSource
			{
				Stream = new Func<CancellationToken, Task<Stream>>(async (arg) => new MemoryStream(byteArray))
			};

			ImageMedia = new Image
			{
				Source = streamSource
			};
			ImageSize = PictureService.GetPictureSize(ImageMedia.Source);
		}

		private void SavePictureCommandExecute()
		{
			var sourceArray = PictureService.ImageToBinary(MediaPath);
			DependencyService.Get<IPictureService>().SavePicture(sourceArray, ImageSize[0], ImageSize[1], MediaPath);
			CoreMethods.PopPageModel(true, false, true);
		}
	}
}


