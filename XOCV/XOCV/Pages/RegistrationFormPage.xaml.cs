using System.Collections.Generic;
using Xamarin.Forms;
using XOCV.Models.ResponseModels;
using XOCV.PageModels;
using XOCV.Pages.Base;

namespace XOCV.Pages
{
    public partial class RegistrationFormPage : XOCVPage
    {
        public List<string> ListOfSurveyorsName { get; set; }
        public List<string> ListOfStores { get; set; }
        public ComplexFormsModel ComplexForm { get; set; }

        public RegistrationFormPage ()
        {
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar (this, false);
        }

        protected override void OnBindingContextChanged ()
        {
            base.OnBindingContextChanged ();

            var pageModel = BindingContext as RegistrationFormPageModel;
            if (pageModel != null) {
                ListOfSurveyorsName = new List<string> ();
                ListOfStores = new List<string> ();
                ComplexForm = pageModel.ComplexForm;
                ListOfSurveyorsName = pageModel.ListOfSurveyorsName;
                ListOfStores = pageModel.ListOfStores;
                storeNumList.ItemSelected += (sender, e) => { searchBar.Unfocus (); };
            }
        }
    }
}