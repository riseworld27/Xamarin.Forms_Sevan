using System.Threading.Tasks;
using XOCV.ViewModels.Base;
using Xamarin.Forms;
using XOCV.Models;
using PropertyChanged;
using XOCV.Interfaces;
using XOCV.Models.ResponseModels;
using System.Windows.Input;
using FreshMvvm;
using XOCV.Helpers;
using XOCV.Pages;

namespace XOCV.PageModels
{
    [ImplementPropertyChanged]
    public class MenuPageModel : BasePageModel
    {
        public ContentModel Content { get; set; }

        public IWebApiHelper WebApiHelper { get; set; }

		public ICommand OpenFormsDetailCommand => new Command<object[]> ((args) => { OpenFormsDetailCommandCommandExecute(args);});
        //public ICommand LogoutCommand => new Command(async () => await LogoutCommandExecute());

        public MenuPageModel () {}
        public MenuPageModel (IWebApiHelper webApiHelper)
        {
            WebApiHelper = webApiHelper;
            Content = WebApiHelper.GetAllContent(Settings.LocalToken).Result;
            //var arg = Content.SetOfForms[0];
            //MessagingCenter.Send(this, "OnDetailChanged1", arg);
			MessagingCenter.Send(this, "OnDetailChanged", Content);
        }

        private void OpenFormsDetailCommandCommandExecute (object [] args)
        {
            MessagingCenter.Send<MenuPageModel, object []> (this, "OnDetailChanged1", args);
        }
        //private async Task LogoutCommandExecute()
        //{
        //    Settings.LocalToken = string.Empty;

        //    var page = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
        //    Application.Current.MainPage = new FreshNavigationContainer(page);
        //    //App.DataBase.ClearAllDataBase(); // ToDo: need to specify it from Kenn
        //}
    }
}