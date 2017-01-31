using System.Windows.Input;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Interfaces;
using XOCV.PageModels.Base;

namespace XOCV.PageModels
{
	[ImplementPropertyChanged]
	public class CarouselPageModel : BasePageModel
	{
		public ImageSource ImageSource { get; set; }
		public string Image { get; set; }
		public string ImageName { get; set; }
		public IPictureService PictureService { get; set; }

		public ICommand EditImageCommand => new Command(() => EditImageCommandExecute());

		public CarouselPageModel()
		{

		}

		public CarouselPageModel(string imageName)
		{
			PictureService = DependencyService.Get<IPictureService>();
			ImageName = imageName;
			Image = PictureService.GetPhotoPath(imageName);

			//Image = "https://cdn.pixabay.com/photo/2016/03/28/12/35/cat-1285634_960_720.png";
			//var byteArray = DependencyService.Get<IPictureService>().ImageToBinary(imageName);

			//System.Diagnostics.Debug.WriteLine("Allocation {0}", byteArray.Length);

			//var streamSource = new StreamImageSource
			//{
			//	Stream = new Func<CancellationToken, Task<Stream>>(async (arg) => new MemoryStream(byteArray))
			//};

			//ImageSource = streamSource;
		}

		public override void Init(object initData)
		{
			base.Init(initData);
			Image = PictureService.GetPhotoPath((string)initData);
		
			//var byteArray = DependencyService.Get<IPictureService>().ImageToBinary((string)initData);
			//System.Diagnostics.Debug.WriteLine("Allocation {0}", byteArray.Length);

			//var streamSource = new StreamImageSource
			//{
				//Stream = new Func<CancellationToken, Task<Stream>>(async (arg) => new MemoryStream(byteArray))
			//};

			//ImageSource = streamSource;
		}

		void EditImageCommandExecute()
		{
			object[] args = { ImageSource, Image };
			Device.BeginInvokeOnMainThread(() => { CoreMethods.PushPageModel<PhotoSignaturePageModel>(args); });
		}
	}
}