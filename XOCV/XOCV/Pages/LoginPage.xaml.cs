using Xamarin.Forms;
using XOCV.Pages.Base;

namespace XOCV.Pages
{
    public partial class LoginPage : XOCVPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}