using System.Collections.ObjectModel;
using FreshMvvm;
using Xamarin.Forms;
using XOCV.Models;
using XOCV.Models.ResponseModels;
using XOCV.PageModels;
using XOCV.PageModels.Popups;

namespace XOCV.Pages.MasterDetailPage
{
	public class MDPage : FreshMasterDetailNavigationContainer
    {
        public MDPage ()
        {
			MessagingCenter.Subscribe<MenuPageModel, ContentModel>(this, "OnDetailChanged", (sender, arg) =>
		    {
			    IsPresented = false;
				var detailPage = FreshPageModelResolver.ResolvePageModel<HomePageModel>(arg.ListOfCompanies);
			    Detail = new FreshNavigationContainer(detailPage);
		    });
			MessagingCenter.Subscribe<MenuPageModel, object []> (this, "OnDetailChanged1", (sender, arg) => 
            {
                IsPresented = false;
                var detailPage = FreshPageModelResolver.ResolvePageModel<FormDetailsPageModel> (arg);
                Detail = new FreshNavigationContainer (detailPage);
            });

            MessagingCenter.Subscribe<DBModel, DBModel> (this, "OnEditCapture", (sender, arg) => 
            {
                IsPresented = false;
				object arguments = new object[] { arg, true };
				var detailPage = FreshPageModelResolver.ResolvePageModel<RegistrationFormPageModel> (arguments);
                Detail = new FreshNavigationContainer (detailPage);
            });

            MessagingCenter.Subscribe<DBModel, DBModel> (this, "OnDeleteCapture", (sender, arg) => 
            {
				var detailPage = FreshPageModelResolver.ResolvePageModel<FormDetailsPageModel> (FormDetailsPageModel.initDataToReturn);
                Detail = new FreshNavigationContainer (detailPage);
            });

			MessagingCenter.Subscribe<PhotoSignaturePageModel>(this, "OnPhotoEdit", (obj) => { IsGestureEnabled = false; });

			MessagingCenter.Subscribe<PhotoSignaturePageModel>(this, "OnPhotoEditFinishing", (obj) => { IsGestureEnabled = true; });

			MessagingCenter.Subscribe<GalleryPageModel>(this, "OnPhotoEdit", (obj) => { IsGestureEnabled = false; });

			MessagingCenter.Subscribe<GalleryPageModel>(this, "OnPhotoEditFinishing", (obj) => { IsGestureEnabled = true; });

			//var detailPage1 = FreshPageModelResolver.ResolvePageModel<HomePageModel>();
			//Detail = new FreshNavigationContainer(detailPage1);

            Master = new MenuPage ();
            MasterBehavior = MasterBehavior.Popover;
        }
    }
}