using Xamarin.Forms;
using XOCV.Pages.Base;

namespace XOCV.Pages
{
    public partial class HomePage : XOCVPage
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
    }
}