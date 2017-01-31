using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Interfaces;
using XOCV.PageModels.Base;

namespace XOCV.PageModels.Popups
{
	[ImplementPropertyChanged]
	public class GalleryPageModel : BasePageModel
	{
        #region Fields
        ObservableCollection<CarouselPageModel> _carouselPages;
        ObservableCollection<string> ListOfImages;
        #endregion

        #region Properties
        public CarouselPageModel CurrentPage { get; set; }
		public ObservableCollection<CarouselPageModel> CarouselPages
		{
			get
			{
				return _carouselPages;
			}
			set
			{
				_carouselPages = value;
				if (CarouselPages.Count > 1)
				{
					CurrentPage = CarouselPages.LastOrDefault();
				}
				else if (CarouselPages.Count == 1)
				{
					CurrentPage = CarouselPages.First();
				}
				else
				{
					CurrentPage = null;
				}
			}
		}
		public string Status { get; set; }
		public string[] Path { get; set; }
		public ObservableCollection<String> ItemImageModels { get; set; }
        #endregion

        #region Services
        public IWebApiHelper WebApiHelper { get; set; }
		public ICameraProvider CameraProvider { get; set; }
		public IPictureService PictureService { get; set; }
		public IUserDialogs UserDialog { get; set; }
        #endregion

        #region Commands
        public ICommand CloseCommand => new Command((obj) => CoreMethods.PopPageModel(false, false, true));
		public ICommand DeleteCommand => new Command((obj) => OnDeleteImage());
		public ICommand ReplaceCommand => new Command(async (obj) => await TakePictureCommandExecute(true));
		public ICommand TakePictureCommand => new Command(async () => await TakePictureCommandExecute());
		public ICommand SelectPictureCommand => new Command(async () => await SelectPictureCommandExecute());
        #endregion

        #region Constructors
        public GalleryPageModel() { }

		public GalleryPageModel(IWebApiHelper webApiHelper, IUserDialogs userDialog, ICameraProvider cameraProvider, IPictureService pictureService)
		{
			WebApiHelper = webApiHelper;
			UserDialog = userDialog;
			CameraProvider = cameraProvider;
			PictureService = pictureService;
		}
        #endregion

        #region Initialize
        public override void Init(object initData)
		{
			base.Init(initData);
            var InitData = initData as object[];
            ListOfImages = InitData[0] as ObservableCollection<string>;
            ItemImageModels = InitData[1] as ObservableCollection<string>;
            Path = InitData[2] as string[];
		}

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
            UpdatePhotos();
        }
        #endregion

        #region Command execution
        public void ImageInterractionCommandExecute()
		{
			ActionSheetConfig ImageAction = new ActionSheetConfig();
			ImageAction.Title = "Photo options";
			ImageAction.Add("Save to gallery", () => SaveImageToGallery());
			ImageAction.Add("Sketch", () => EditImage());
			//ImageAction.Add("Replace", async () => await TakePictureCommandExecute());
			//ImageAction.Add("Delete", () => OnDeleteImage());
			UserDialog.ActionSheet(ImageAction);
		}

        private async Task<Models.MediaFile> TakePictureCommandExecute(bool isReplace = false)
        {
            try
            {
                return await CameraProvider.TakePhotoAsync(new Helpers.CameraMediaStorageOptions
                {
                    DefaultCamera = Helpers.CameraDevice.Rear,
                    MaxPixelDimension = 400
                }).ContinueWith(
                    t =>
                    {
                        if (t.IsFaulted)
                        {
                            Status = t.Exception.InnerException.ToString();
                            UserDialog.AlertAsync("Taking photo failed!", "Warning!", "Ok");
                        }
                        else if (t.IsCanceled)
                        {
                            Status = "Canceled";
                        }
                        else
                        {
                            int imagePosition = -1;
                            if (isReplace)
                            {
                                imagePosition = CarouselPages.IndexOf(CurrentPage);
                            }
                            var mediaFile = t.Result;
                            SavePhoto(mediaFile, imagePosition).Wait();
                            GC.Collect();
                            return mediaFile;
                        }
                        return null;
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await UserDialog.AlertAsync("Taking photo failed!", "Warning!", "Ok");
                return null;
            }
        }

        private async Task<Models.MediaFile> SelectPictureCommandExecute()
        {
            try
            {
                return await CameraProvider.SelectPhotoAsync(new Helpers.CameraMediaStorageOptions
                {
                    DefaultCamera = Helpers.CameraDevice.Front,
                    MaxPixelDimension = 400
                }).ContinueWith(
                    t =>
                    {
                        if (t.IsFaulted)
                        {
                            Status = t.Exception.InnerException.ToString();
                            UserDialog.AlertAsync("Taking photo failed!", "Warning!", "Ok");
                        }
                        else if (t.IsCanceled)
                        {
                            Status = "Canceled";
                        }
                        else
                        {
                            var mediaFile = t.Result;
                            int imagePosition = -1;
                            SavePhoto(mediaFile, imagePosition).Wait();
                            GC.Collect();
                            return mediaFile;
                        }
                        return null;
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await UserDialog.AlertAsync("Taking photo failed!", "Warning!", "Ok");
                return null;
            }

            return null;
        }

        private void OnDeleteImage()
        {
            try
            {
                if (CarouselPages != null && CarouselPages.Count > 0)
                {
                    ///CarouselPages.Remove(CurrentPage);
                    ListOfImages.Remove(ListOfImages.Where(i => i == CurrentPage.ImageName).First());
                    ItemImageModels.Remove(ItemImageModels.Where(i => i == CurrentPage.Image).First());
                    //CurrentPage = CarouselPages.FirstOrDefault();
                    UpdatePhotos();
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                UserDialog.Alert(message, "Error!", "Close");
            }
        }

        #endregion

        #region Additional methods
        public void SaveImageToGallery()
		{
			string fileName = CurrentPage.Image;
			PictureService.SavePictureToGallery(fileName);
		}

		//public void OpenImageCommandExecute()
		//{
		//	string Medias = CurrentPage.Image;
		//	CoreMethods.PushPageModel<PhotoPageModel>(Medias);
		//}

		public void EditImage()
		{
			if (CurrentPage == null) return;

			string Medias = CurrentPage.Image;
			Device.BeginInvokeOnMainThread(() => { CoreMethods.PushPageModel<PhotoSignaturePageModel>(Medias); });
		}

        public async Task SavePhoto(Models.MediaFile mediaFile, int imagePosition = -1)
        {
            try
            {
                if (imagePosition != -1)
                {
                    string fileName = string.Format("{0}_{1}_{2}_{3}",
                    Path[0],
                    Path[1],
                    Path[2],
                    DateTime.Now.ToString("yy-MM-dd_hh-mm-ss"));

                    await PictureService.SavePictureToDisk(ImageSource.FromStream(() => mediaFile.Source), fileName);

                    var image = PictureService.GetPictureFromDisk(fileName);

                    int index1 = ListOfImages.IndexOf(CurrentPage.ImageName);
                    int index2 = ItemImageModels.IndexOf(CurrentPage.Image);
                    ListOfImages.RemoveAt(index1);
                    ListOfImages.Insert(index1, fileName + ".jpg");
                    ItemImageModels.RemoveAt(index2);
                    ItemImageModels.Insert(index2, image);
                }
                else
                {
                    string fileName = string.Format("{0}_{1}_{2}_{3}",
                    Path[0],
                    Path[1],
                    Path[2],
                    DateTime.Now.ToString("yy-MM-dd_hh-mm-ss"));

                    await PictureService.SavePictureToDisk(ImageSource.FromStream(() => mediaFile.Source), fileName);

                    var image = PictureService.GetPictureFromDisk(fileName);

                    ListOfImages.Add(fileName + ".jpg");
                    ItemImageModels.Add(image);
                }
                UpdatePhotos();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await UserDialog.AlertAsync("Saving photo failed!", "Warning!", "Ok");
            }
        }

        private void UpdatePhotos()
        {
            try
            {
                var carouselPages = new ObservableCollection<CarouselPageModel>();
                foreach (var image in ListOfImages)
                {
                    carouselPages.Add(new CarouselPageModel(image));
                }
                if (CarouselPages != null)
                {
                    CarouselPages.Clear();
                }
                MessagingCenter.Send<GalleryPageModel>(this, "onCarouselChanged");
                CarouselPages = carouselPages;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                UserDialog.Alert("Saving photo failed!", "Warning!", "Ok");
            }
        }
        #endregion

        #region Page states
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            UpdatePhotos();
            base.ViewIsAppearing(sender, e);
            MessagingCenter.Send<GalleryPageModel>(this, "OnPhotoEdit");
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            MessagingCenter.Send<GalleryPageModel>(this, "OnPhotoEditFinishing");
        }
        #endregion
    }
}