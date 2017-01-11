using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XOCV.Views;

namespace XOCV
{
	//public class CarouselLayoutView : ContentView
	//{

	//	public CarouselLayoutView()
	//	{
	//		BackgroundColor = Color.White;

	//		var FonImage = new CachedImage
	//		{
	//			Aspect = Aspect.AspectFit,
	//			HorizontalOptions = LayoutOptions.FillAndExpand,
	//			VerticalOptions = LayoutOptions.FillAndExpand,
	//			CacheDuration = TimeSpan.FromDays(30),
	//			DownsampleToViewSize = true,
	//			DownsampleUseDipUnits = true,
	//			RetryCount = 3,
	//			RetryDelay = 250
	//		};

	//		FonImage.Success += (sender, e) =>
	//		{
	//			var h = e.ImageInformation.OriginalHeight;
	//			var w = e.ImageInformation.OriginalHeight;
	//		};


	//		FonImage.SetBinding(CachedImage.SourceProperty, new Binding("ImageSource"));

	//		//var readMoreButton = new Button()
	//		//{
	//		//	Text = "READ MORE",
	//		//	BackgroundColor = Color.FromHex("3CA7E2"),
	//		//	TextColor = Color.White,
	//		//	HeightRequest = 40,
	//		//	WidthRequest = 150,
	//		//	BorderRadius = 0,
	//		//	HorizontalOptions = LayoutOptions.Center,
	//		//	VerticalOptions = LayoutOptions.End,
	//		//};

	//		//readMoreButton.SetBinding(Button.CommandProperty, new Binding("LoadMoreCommand"));

	//		Content = new Grid
	//		{
	//			Children =
	//			{
	//				FonImage,
	//				//new StackLayout
	//				//{
	//				//	VerticalOptions = LayoutOptions.End,
	//				//	HorizontalOptions = LayoutOptions.FillAndExpand,
	//				//	Padding = new Thickness (70,85),
	//				//	Spacing = 25,
	//				//	//HorizontalOptions = LayoutOptions.FillAndExpand,
	//				//	Children =
	//				//	{
	//				//		readMoreButton
	//				//	}
	//				//}
	//			}
	//		};
	//	}


	//}

	public class CarouselLayoutView : ContentView
	{

		public CarouselLayoutView()
		{
			BackgroundColor = Color.White;

			var FonImage = new CachedImage
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			FonImage.SetBinding(CachedImage.ImageUrlProperty, new Binding("Image"));

			Content = new Grid
			{
				Children =
				{
					FonImage,
				}
			};
		}
	}
}
