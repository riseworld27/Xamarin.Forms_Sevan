using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using SQLite.Net.Attributes;
using Xamarin.Forms;
using XOCV.Enums;

namespace XOCV.Models
{
    public class DBModel
    {
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }
		public long FormID { get; set; }
        public bool IsSelected { get; set; }
		public bool IsTemplateForm { get; set; } 
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string StoreNumber { get; set; }
        public FormStatus FormStatus { get; set; }
        public SyncStatus SyncStatus { get; set; }

        #region Additionals
        [Ignore]
        public ICommand EditCaptureCommand => new Command (async () => await EditCaptureCommandExecute ());

        [Ignore]
        public ICommand DeleteCaptureCommand => new Command (async () => await DeleteCaptureCommandExecute ());

        private async Task EditCaptureCommandExecute ()
        {
            DBModel model = App.DataBase.GetContentById (ID);
			//int[] args = new int[] { FormID};
            MessagingCenter.Send<DBModel, DBModel> (this, "OnEditCapture", model);
        }

        private async Task DeleteCaptureCommandExecute ()
        {
            var result = await UserDialogs.Instance.ConfirmAsync("Do you really want to delete this record? It can’t be undone","Warning!", "Yes", "No");

            if (result)
            {
                DBModel model = App.DataBase.GetContentById(ID);
                App.DataBase.DeleteItem(model);
                MessagingCenter.Send<DBModel, DBModel>(this, "OnDeleteCapture", model);
            }
        }
        #endregion
    }
}