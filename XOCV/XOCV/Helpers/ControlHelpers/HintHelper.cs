using Xamarin.Forms;
using XOCV.Helpers.ControlHelpers.Base;
using XOCV.Models.ResponseModels;
using XOCV.Extensions;
using XOCV.Views;

namespace XOCV.Helpers.ControlHelpers
{
    public class HintHelper : BaseControlHelper
    {
        public static StackLayout SetHintProperties (Item model)
        {
			StackLayout mainStack = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				BackgroundColor = Color.FromHex("#b1e3ac"),
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			if (!model.Properties.Text.IsNullOrEmpty())
			{
				StackLayout stack = new StackLayout
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Start
				};

				Label label = new Label();
				label.Margin = new Thickness(0, 5, 5, 5);
				label.Text = model.Properties.Text;
				label.FontSize = 18;
				label.TextColor = Color.Black;
				label.BackgroundColor = Color.Transparent;
				label.HorizontalOptions = LayoutOptions.StartAndExpand;
				label.VerticalOptions = LayoutOptions.CenterAndExpand;
				label.LineBreakMode = LineBreakMode.WordWrap;
				stack.Children.Add(label);
				mainStack.Children.Add(stack);
			}
			if (!model.Properties.LocalImagePath.IsNullOrEmpty())
			{
				var image = new CachedImage
				{
					ImageUrl = model.Properties.LocalImagePath,
					HorizontalOptions = LayoutOptions.Center,
					WidthRequest = 200,
					HeightRequest = 200
				};
				mainStack.Children.Add(image);
			}
            return mainStack;
        }
    }
}