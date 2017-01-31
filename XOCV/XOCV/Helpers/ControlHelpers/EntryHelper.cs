using System.Linq;
using Xamarin.Forms;
using XOCV.Helpers.ControlHelpers.Base;
using XOCV.Models.ResponseModels;
using XOCV.Resources;
using XOCV.Views;

namespace XOCV.Helpers.ControlHelpers
{
    public class EntryHelper : BaseControlHelper
    {
		public static AutoresizedEditor SetEntryProperties(Item model)
        {
            AutoresizedEditor editor = new AutoresizedEditor();
            editor.HorizontalOptions = LayoutOptions.FillAndExpand;
			editor.BackgroundColor = Color.White;
            editor.TextColor = Color.Black;
            editor.FontSize = 18;
			editor.Keyboard = model.Properties.KeyboardType == Enums.KeyboardType.Numeric ? Keyboard.Numeric : Keyboard.Default;

            return editor;
        }
    }
}