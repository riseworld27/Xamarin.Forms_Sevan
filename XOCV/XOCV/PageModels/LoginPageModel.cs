using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Helpers;
using XOCV.Interfaces;
using XOCV.Models;
using XOCV.Pages.MasterDetailPage;
using XOCV.PageModels.Base;

namespace XOCV.PageModels
{
    [ImplementPropertyChanged]
    public class LoginPageModel : BasePageModel
    {
        #region Properties
        public LoginModel Login { get; set; }
        public bool IsSelectedRememberMe { get; set; }
        public string Version { get; } = "Version 1.1.3";
        #endregion

        #region Services
        public IWebApiHelper WebApiHelper { get; set; }
        public IUserDialogs Dialogs { get; set; }
        public INetworkConnection NetworkConnection { get; set; }
        #endregion

        #region Commands
        public ICommand LoginCommand => new Command (async () => await LoginCommandExecute ());
        public ICommand SelectCheckBoxCommand => new Command(SelectCheckBoxCommandExecute);
        public ICommand OpenVersionDescriptionCommand => new Command(async () => await OpenVersionDescriptionCommandExecute());
        #endregion

        #region Initialize
        public override void Init (object initData)
        {
            Login = new LoginModel ();

            if (!string.IsNullOrEmpty(Settings.LocalLogin) && !string.IsNullOrEmpty(Settings.LocalPassword))
            {
                Login.UserName = Settings.LocalLogin;
                Login.Password = Settings.LocalPassword;
            }
			IsSelectedRememberMe = true;

            #if DEBUG
            // TEST ACCOUNT
            Login.UserName = "admin";
            Login.Password = "123qwe";
            #endif
        }
        #endregion

        #region Constructors
        public LoginPageModel () { }
        public LoginPageModel (IWebApiHelper webApiHelper, IUserDialogs dialogService, INetworkConnection networkConnection)
        {
            WebApiHelper = webApiHelper;
            Dialogs = dialogService;
            NetworkConnection = networkConnection;
        }
        #endregion

        #region Command execution
        private async Task LoginCommandExecute ()
        {
            Dialogs.ShowLoading ("Loading");
            try
            {
                if (NetworkConnection.IsConnected)
                {
                    if (!string.IsNullOrEmpty (Login.UserName) && !string.IsNullOrEmpty (Login.Password))
                    {
                        var result = await WebApiHelper.Authorization (Login);
                        if (result.Token != null)
                        {
                            Settings.LocalLogin = IsSelectedRememberMe ? Login.UserName : string.Empty;
                            Settings.LocalPassword = IsSelectedRememberMe ? Login.Password : string.Empty;
                            Application.Current.MainPage = new MDPage ();
                        }
                        else
                        {
                            Dialogs.HideLoading ();
                            await Dialogs.AlertAsync (result.Error.Details, result.Error.Message, "OK");
                        }
                    }
                    else
                    {
                        Dialogs.HideLoading ();
                        await Dialogs.AlertAsync ("Username or password cannot be empty!", "Warning!", "OK");
                    }
                }
                else
                {
                    if ((!string.IsNullOrEmpty (Login.UserName) 
                        && !string.IsNullOrEmpty (Login.Password)) 
                        && (Login.UserName == Settings.LocalLogin 
                        && Login.Password == Settings.LocalPassword))
                    {
                        Application.Current.MainPage = new MDPage ();
                    }
                    else
                    {
                        Dialogs.HideLoading ();
                        await Dialogs.AlertAsync ("Username or password is incorrect!", "Warning!", "OK");
                    }
                }
                Dialogs.HideLoading ();
            }
            catch (Exception ex)
            {
                Debug.WriteLine (ex.Message);
                Dialogs.HideLoading ();
                #if DEBUG
                await Dialogs.AlertAsync(ex.Message, "Error!", "OK");
                #else
                await Dialogs.AlertAsync ("Internal error!", "Warning!", "OK");
                #endif
            }
        }
        private void SelectCheckBoxCommandExecute()
        {
            IsSelectedRememberMe = !IsSelectedRememberMe;
        }
        private async Task OpenVersionDescriptionCommandExecute()
        {
            await Dialogs.AlertAsync(Version, "Version description", "Close");
        }
#endregion
    }
}