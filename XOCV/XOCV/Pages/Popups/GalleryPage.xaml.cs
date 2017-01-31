using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Xamarin.Forms;
using XOCV.PageModels.Popups;
using XOCV.Pages.Base;
using XOCV.Views;

namespace XOCV.Pages.Popups
{
	public partial class GalleryPage : XOCVPage
	{
		public GalleryPage()
		{
			//NavigationPage.SetHasNavigationBar(this, false);
			InitializeComponent();
			carousel.ItemTemplate = new DataTemplate(typeof(CarouselLayoutView));
			MessagingCenter.Subscribe<GalleryPageModel>(this, "onCarouselChanged", (obj) => { RefreshCarousel(); });
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			var pageModel = BindingContext as GalleryPageModel;
			carousel.LongPressActivated += (sender, e) => { pageModel.ImageInterractionCommandExecute(); };
		}

		void RefreshCarousel()
		{
		    try
		    {
		        carousel = null;
		        pageIndicator = null;
		        carousel = new CarouselLayout
		        {
		            HorizontalOptions = LayoutOptions.FillAndExpand,
		            VerticalOptions = LayoutOptions.FillAndExpand
		        };
		        pageIndicator = new PagerIndicatorDots {DotSize = 10, DotColor = Color.Green};
		        carousel.SetBinding(CarouselLayout.ItemsSourceProperty, new Binding("CarouselPages", BindingMode.TwoWay));
		        carousel.SetBinding(CarouselLayout.SelectedItemProperty, new Binding("CurrentPage", BindingMode.TwoWay));
		        pageIndicator.SetBinding(PagerIndicatorDots.ItemsSourceProperty,
		            new Binding("CarouselPages", BindingMode.TwoWay));
		        pageIndicator.SetBinding(PagerIndicatorDots.SelectedItemProperty,
		            new Binding("CurrentPage", BindingMode.TwoWay));
		    }
		    catch (Exception e)
		    {
		        string message = e.Message;
		        UserDialogs.Instance.Alert(e.Message, "Error!", "Close");
		    }
        }
	}
}