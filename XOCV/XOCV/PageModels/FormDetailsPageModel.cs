using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Interfaces;
using XOCV.Models;
using XOCV.Models.ResponseModels;
using XOCV.Services;
using XOCV.ViewModels.Base;

namespace XOCV.PageModels
{
    [ImplementPropertyChanged]
    public class FormDetailsPageModel : BasePageModel
    {
        #region Fields
        private FormModel _tempForm;
        private bool _isAllItemsSelected;
		private long formId;
		public static object initDataToReturn;
        #endregion

        #region Properties
        public bool IsFirstLoadEditCapture { get; set; } = true;
        public ComplexFormsModel AvailableForms { get; set; }
        public List<CaptureModel> Captures { get; set; }
        public DBModel DbModel { get; set; }
        public List<DBModel> DbModels { get; set; }
        public bool IsAllItemsSelected
        {
            get
            {
                return _isAllItemsSelected;
            }
            set
            {
                _isAllItemsSelected = value;
                if (DbModels.Any())
                {
                    if (IsAllItemsSelected == true)
                    {
                        foreach (var item in DbModels)
                        {
							if (item.FormID == formId)
							{
								item.IsSelected = true;
								App.DataBase.SaveContent(item);
							}
                        }
                    }
                    else if (IsAllItemsSelected == false)
                    {
                        foreach (var item in DbModels)
                        {
							if (item.FormID == formId)
							{
								item.IsSelected = false;
								App.DataBase.SaveContent(item);
							}
                        }
                    }
                    var dbModels = App.DataBase.GetContent();
					dbModels = dbModels.Where(item => item.FormID == formId).ToList();
					DbModels = dbModels.Where(i => i.IsTemplateForm == false).ToList();
                    //DbModels.RemoveAt(0);
                }
                else IsAllItemsSelected = false;
            }
        }

        #endregion

        #region Services
        public IWebApiHelper WebApiHelper { get; }
        public IUserDialogs Dialogs { get; }
        public INetworkConnection NetworkConnectionService { get; }
        public IPictureService PictureService { get; }
        public IFtpService FtpService { get; }
        #endregion

        #region Commands
        public ICommand AddNewCaptureCommand => new Command (async () => await AddNewCaptureCommandExecute ());
        public ICommand SyncCommand => new Command (async () => await SyncCommandExecute ());
        public ICommand MakeBackUpCommand => new Command(async () => await MakeBackUpCommandExecute());
        #endregion

        #region Constructors
        public FormDetailsPageModel () {}
        public FormDetailsPageModel (IWebApiHelper webApiHelper, IUserDialogs dialogService, INetworkConnection networkConnectionService, IPictureService pictureService, IFtpService ftpService)
        {
            WebApiHelper = webApiHelper;
            Dialogs = dialogService;
            NetworkConnectionService = networkConnectionService;
            PictureService = pictureService;
            FtpService = ftpService;
        }
        #endregion

        #region Initialize
        public override void Init (object initData)
        {
			FormDetailsPageModel.initDataToReturn = initData;
			object[] InitData = initData as object[];
            AvailableForms = InitData[0] as ComplexFormsModel;
			formId = (long)InitData[1];
            DbModel = new DBModel ();
			DbModels = App.DataBase.GetContent ().Where(m => m.FormID == formId).ToList();
            foreach (var item in DbModels)
                item.IsSelected = false;
			DbModels.Remove (DbModels.Where (x => x.IsTemplateForm == true).FirstOrDefault());
        }
        #endregion

        #region Commands execution
        private async Task AddNewCaptureCommandExecute ()
        {
			var dbModels = App.DataBase.GetContent().Where(m => m.FormID == formId).ToList();
            DbModel = dbModels.Where(x => x.IsTemplateForm == true).First();
			object arg = new object[] { DbModel, false };
            await CoreMethods.PushPageModel<RegistrationFormPageModel> (arg);
        }

        private async Task SyncCommandExecute ()
        {
            if (NetworkConnectionService.IsConnected)
            {
                foreach (var item in DbModels)
                    App.DataBase.SaveContent(item);
                int syncedCount = 0;
                var items = App.DataBase.GetAllComplexFormContent(getOnlySelected: true);
                int itemsCount = items.Count ();
                if (items.Count == 0)
                {
                    await Dialogs.AlertAsync ("Nothing to sync!", null, "Ok!");
                    return;
                }
                await Task.Run (() => Dialogs.ShowLoading ("Starting sync..."));
                foreach (var item in items)
                {
                    await Task.Run (() => Dialogs.ShowLoading ($"Sync form... ({syncedCount} of {itemsCount})"));
					var content = new ProgramModel ();
                    foreach (var captureModel in item.Captures)
                    {
                        if (captureModel.SyncStatus == Enums.SyncStatus.NotSync)
                        {
                            content.SetOfForms.Add(item);
                            bool success = await WebApiHelper.PostAllContent(content);
                            if (!success)
                            {
                                await Task.Run(() => Dialogs.HideLoading());
                                await Dialogs.AlertAsync("Sync for photos is failed!", "Warning!", "Ok!");
                                return;
                            }
                            try
                            {
                                var resultForBackUp = await FtpService.SendJsonFile(item);
                                if (resultForBackUp == false)
                                {
                                    await UserDialogs.Instance.AlertAsync("Sync for json is failed!", "WARNING!", "OK");
                                }
                            }
                            catch (Exception ex)
                            {
                                var resEx = ex.Message;
                                await UserDialogs.Instance.AlertAsync("FTP is not available!", "WARNING!", "OK");
                                return;
                            }
                            syncedCount = success ? syncedCount + 1 : syncedCount;
                            await Task.Run(() => Dialogs.ShowLoading("Sync images... "));
                            foreach (var poll in content.SetOfForms)
                            {
                                foreach (var form in poll.Forms)
                                {
									foreach (var complexItemCollection in form.MultiComplexItems)
                                    {
										foreach (var complexItem in complexItemCollection.ComplexItems)
                                        {
											foreach (var formItem in complexItem.Items)
											{
												if (formItem.Name == "imageKey")
												{
													try
													{
													    var imagesForSyncing = formItem.Images.ToList();

                                                        if (imagesForSyncing.Count <= 0) continue;

													    var syncImagesResult = await PictureService.SyncImages(imagesForSyncing);
													    if (syncImagesResult == false)
													    {
													        await Task.Run(() => Dialogs.HideLoading());
													        await Dialogs.AlertAsync("Sync for photos is failed!", "Warning!", "Ok!");
													        return;
													    }
													}
													catch (Exception ex)
													{
														var resEx = ex.Message;
														await UserDialogs.Instance.AlertAsync("FTP is not available!", "WARNING!", "OK");
														return;
													}
												}
											}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                await Task.Run (() => Dialogs.HideLoading ());
                if (syncedCount == itemsCount)
                {
                    DbModel = new DBModel();
					DbModels = App.DataBase.GetContent().Where(m => m.FormID == formId).ToList();
					DbModels.Remove(DbModels.Where(x => x.IsTemplateForm == true).FirstOrDefault());
                    await Dialogs.AlertAsync ("All data synchronized successfully!", "Success!", "Ok!");
                }
                else
                {
                    await Dialogs.AlertAsync("Sync failed!\nNot all data synced successfully!", "Warning!", "Ok!");
                }
            }
            else
            {
                await Dialogs.AlertAsync ("Network in unavailable!\nPlease connect to your network!", null, "Ok!");
            }
        }

        private async Task MakeBackUpCommandExecute()
        {
            Dialogs.ShowLoading("Making backup...");
            var modelLocalDbContent = App.DataBase.GetContent().FirstOrDefault().Content;
            if (!string.IsNullOrEmpty(modelLocalDbContent))
            {
                var resultBackUp = await FtpService.BackUpAllLocalDataBase(modelLocalDbContent);
                Dialogs.Alert(resultBackUp? "Success!" : "Failed!", "Backup");
            }
            Dialogs.HideLoading();
        }
        #endregion
    }
}