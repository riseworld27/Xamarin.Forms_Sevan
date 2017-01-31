using Xamarin.Forms;
using XOCV.Pages.Base;

namespace XOCV.Pages
{
    public partial class FormDetailsPage : XOCVPage
    {
        public FormDetailsPage ()
        {
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);
        }
    }
}