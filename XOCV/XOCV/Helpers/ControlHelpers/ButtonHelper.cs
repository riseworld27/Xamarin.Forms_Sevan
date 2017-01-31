using Xamarin.Forms;
using XOCV.Helpers.ControlHelpers.Base;
using XOCV.Models.ResponseModels;
using XOCV.Views;

namespace XOCV.Helpers.ControlHelpers
{
    public class ButtonHelper : BaseControlHelper
    {
        public static CustomButton SetPolButtonProperties (ComplexFormsModel model)
        {
            CustomButton button = new CustomButton ();

            button.Text = model.FormsTitle;
            button.HorizontalOptions = LayoutOptions.Start;
            button.VerticalOptions = LayoutOptions.Center;
            button.TextColor = Color.FromHex ("0B2D90");
            button.BackgroundColor = Color.White;
            button.FontFamily = "Roboto";
            button.CommandParameter = model;
            button.FontSize = 20;
            button.SetBinding (Button.CommandProperty, new Binding ("OpenFormsDetailCommand"));
			long formId = model.FormId;
			object[] args = { model, formId };
			button.CommandParameter = args;

            return button;
        }
    }
}