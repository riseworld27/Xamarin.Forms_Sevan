using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XOCV.Pages.Base;
using SignaturePad.Forms;
using XOCV.PageModels;

namespace XOCV.Pages
{
	public partial class PhotoSignaturePage : XOCVPage
	{
		private Dictionary<string, Color> ColorPallete;
		public PhotoSignaturePageModel pageModel;
		public int[] ImageSize
		{
			get { return (int[])GetValue(ImageSizeProperty); }
			set { SetValue(ImageSizeProperty, value); }
		}

		public static readonly BindableProperty ImageSizeProperty =
			BindableProperty.Create(
				nameof(ImageSize),
				typeof(object),
				typeof(PollPage),
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) =>
				{

				}
			);
		public PhotoSignaturePage()
		{
			NavigationPage.SetHasNavigationBar(this, true);
			InitializeComponent();

			BuildColorPallete();
        }

		private static Color GetTextColor(Color backgroundColor)
		{
			var backgroundColorDelta = ((backgroundColor.R * 0.3) + (backgroundColor.G * 0.6) + (backgroundColor.B * 0.1));

			return (backgroundColorDelta > 0.4f) ? Color.Black : Color.White;
		}

		private void BuildColorPallete()
		{
			ColorPallete = new Dictionary<string, Color>
			{
				{ "White", Color.White },
				{ "Silver", Color.Silver },
				{ "Gray", Color.Gray },
				{ "Yellow", Color.Yellow },
				{ "Aqua", Color.Aqua },
				{ "Blue", Color.Blue },
				{ "Navy", Color.Navy },
				{ "Lime", Color.Lime },
				{ "Green", Color.Green },
				{ "Teal", Color.Teal },
				{ "Olive", Color.Olive },
				{ "Fuschia", Color.Fuchsia },
				{ "Red", Color.Red },
				{ "Purple", Color.Purple },
				{ "Maroon", Color.Maroon },
				{ "Black", Color.Black },
			};

			foreach (var button in ColorPallete.Select(color => new Button
			{
				WidthRequest = 40,
				HeightRequest = 40,
				BorderRadius = 20,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Center,
				BackgroundColor = color.Value,
			}))
			{
				button.Clicked += OnButtonClicked;

				colorPaletteStack.Children.Add(button);
			}

		}

		private void OnButtonClicked(object sender, EventArgs e)
		{
			var button = (Button)sender;

			sketchImage.CurrentLineColor = button.BackgroundColor;
		}

		protected override void OnBindingContextChanged()
		{
		    base.OnBindingContextChanged();
		    this.SetBinding(ImageSizeProperty, new Binding("ImageSize", BindingMode.TwoWay));
		    pageModel = BindingContext as PhotoSignaturePageModel;

		    if (pageModel != null)
		    {
                sourceImage.WidthRequest = pageModel.ImageSize[0];
                sourceImage.HeightRequest = pageModel.ImageSize[1];
                sketchImage.WidthRequest = pageModel.ImageSize[0];
                sketchImage.HeightRequest = pageModel.ImageSize[1];
            }
        }
	}
}