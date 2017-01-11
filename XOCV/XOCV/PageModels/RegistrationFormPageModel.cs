using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Enums;
using XOCV.Models;
using XOCV.Extensions;
using XOCV.Models.ResponseModels;
using XOCV.ViewModels.Base;
using Newtonsoft.Json;

namespace XOCV.PageModels
{
    [ImplementPropertyChanged]
    public class RegistrationFormPageModel : BasePageModel
    {
        #region Fields
        private string _searchStore;
        private StoreNumberModel _selectedStoreNumber;
        #endregion

        #region Properties
        public ComplexFormsModel ComplexForm { get; set; }
        public CaptureModel Capture { get; set; }
        public List<string> ListOfSurveyorsName { get; set; }
        public List<string> ListOfStores { get; set; }
        public StoreNumberModel StoreNumber { get; set; }
        public bool IsListOfStoreNumber { get; set; } = false;
		public bool isEdit = false;
        public string SearchStore
        {
            get { return _searchStore; }
            set
            {
                _searchStore = value;
                IsListOfStoreNumber = true;
                SearchStoreCommandExecute ();
            }
        }
        public List<StoreNumberModel> RezStoreNumberModels { get; set; }
        public StoreNumberModel SelectStoreNumber
        {
            get { return _selectedStoreNumber; }
            set
            {
                _selectedStoreNumber = value;
                if (_selectedStoreNumber != null)
                {
                    StoreNumber = _selectedStoreNumber;
                    SearchStore = String.Empty;
                    IsListOfStoreNumber = false;
                }
            }
        }
        public int IndexOfSurveyor { get; set; }
        public int IndexOfStore { get; set; }
        public int BrandingPackageValue { get; set; }
        public DBModel DbModel { get; set; }
        public string Surveyor { get; set; }
        #endregion

        #region Commands
        public ICommand OpenNewCaptureCommand => new Command (async () => await OpenNewCaptureCommandExecute ());
        public ICommand SearchStoreCommand => new Command (SearchStoreCommandExecute);
        #endregion

        #region Constructor
        public RegistrationFormPageModel () { }
        #endregion

        #region Initialize
        public override void Init (object initData)
        {
			object[] data = initData as object[];
            DbModel = data[0] as DBModel;
			isEdit = (bool)data[1];
            ComplexForm = JsonConvert.DeserializeObject<ComplexFormsModel> (DbModel?.Content);
            ListOfSurveyorsName = new List<string> ();
            ListOfStores = new List<string> ();

            RezStoreNumberModels = new List<StoreNumberModel> ();
            SelectStoreNumber = new StoreNumberModel ();
            SearchStore = string.Empty;
            Surveyor = ComplexForm.Surveyors.First ().Name;

            if (ComplexForm == null) return;

            foreach (var surveyorModel in ComplexForm.Surveyors.Where(surveyorModel => surveyorModel.Name != null))
            {
                ListOfSurveyorsName.Add (surveyorModel.Name);
            }

            foreach (var storeNumberModel in ComplexForm.StoreNumbers)
            {
                ListOfStores.Add (storeNumberModel.StoreNumber.ToString ());
            }

            if (ComplexForm.Captures.Count != 0)
            {
                StoreNumber = ComplexForm.StoreNumbers.Find (x => x.StoreNumber.ToString () == ComplexForm.Captures.First ().StoreNumber);
            }

            if (ComplexForm.Surveyor != null)
            {
                IndexOfSurveyor = ListOfSurveyorsName.FindIndex (x => x.Contains (ComplexForm.Surveyor));
            }
            else
            {
                IndexOfSurveyor = -1;
            }
        }
        #endregion

        #region Commands execution
        private async Task OpenNewCaptureCommandExecute ()
        {
			string o = JsonConvert.SerializeObject(DbModel);
			var dbModel = JsonConvert.DeserializeObject<DBModel>(o);
			if (!isEdit)
			{
				dbModel.ID = App.DataBase.GetContent().Count + 2;
			}

            foreach (var form in ComplexForm.Forms)
            {
                if (SelectStoreNumber != null)
                {
                    form.StoreNumber = SelectStoreNumber.StoreNumber.ToString ();
                }
            }

            if (SelectStoreNumber != null)
            {
                ComplexForm.StoreNumber = SelectStoreNumber.StoreNumber.ToString ();
                ComplexForm.StoreNum = SelectStoreNumber.StoreNumber;
                dbModel.StoreNumber = SelectStoreNumber.StoreNumber.ToString ();
            }

            if (IndexOfSurveyor != -1)
            {
                ComplexForm.Surveyor = ListOfSurveyorsName [IndexOfSurveyor];
            }

            var captID = 0;

            if (ComplexForm.Captures.Count != 0)
            {
                captID = ComplexForm.Captures.First ().CaptureModelID;
            }

            if (ComplexForm.CaptureGuid == Guid.Empty)
            {
                ComplexForm.CaptureGuid = Guid.NewGuid();
            }

            if (ComplexForm.Captures.Count == 0)
            {
                Capture = new CaptureModel
                {
                    CaptureModelID = captID,
                    Date = DateTime.Now,
                    StoreNumber = ComplexForm.StoreNumber,
                    FormStatus = FormStatus.Incomplete,
                    SyncStatus = SyncStatus.NotSync,
                };
                ComplexForm.Captures = new List<CaptureModel> ();
                ComplexForm.Captures.Add (Capture);
                ComplexForm.UserId = 2;
                dbModel.Date = Capture.Date;
                dbModel.StoreNumber = Capture.StoreNumber;
                dbModel.SyncStatus = Capture.SyncStatus;
                dbModel.FormStatus = Capture.FormStatus;
				dbModel.IsTemplateForm = false;
            }
            dbModel.Content = JsonConvert.SerializeObject (ComplexForm);
            App.DataBase.SaveContent (dbModel);
            await CoreMethods.PushPageModel<PollPageModel> (dbModel);
        }
        private void SearchStoreCommandExecute ()
        {
            if (!SearchStore.IsNullOrEmpty())
            {
                RezStoreNumberModels = ComplexForm.StoreNumbers.Where (x => x.StoreNumber.ToString ().StartsWith (SearchStore)).ToList ();
            }
        }
        #endregion
    }
}