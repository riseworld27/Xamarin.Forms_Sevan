using System.Linq;
using Xamarin.Forms;
using XOCV.Helpers.ControlHelpers.Base;
using XOCV.Models.ResponseModels;
using XOCV.Resources;

namespace XOCV.Helpers.ControlHelpers
{
    public class LabelHelper : BaseControlHelper
    {
        public static Label SetLabelProperties(Item model)
        {
            Label label = new Label();

            label.Text = model.Properties.Text;
            //label.HorizontalOptions = GetPositionOption(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.HorizontalOptions).Value);
            //label.VerticalOptions = GetPositionOption(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.VerticalOptions).Value);
            //label.FontSize = GetNumericValue(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.FontSize).Value);
            //label.FontAttributes = GetFontAttributes(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.FontAttributes).Value);
            //label.HorizontalTextAlignment = GetTextAlignmentPosition(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.HorizontalTextAlignment).Value);
            //label.VerticalTextAlignment = GetTextAlignmentPosition(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.VerticalTextAlignment).Value);
            //label.LineBreakMode = GetLineBreakMode(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.LineBreakMode).Value);
            //label.TextColor = GetColor(model.Properties.FirstOrDefault(x => x.Key == XOCVRes.TextColor).Value);
            label.FontSize = 20;
            label.TextColor = Color.Black;
            label.BackgroundColor = Color.Transparent;
            label.LineBreakMode = LineBreakMode.WordWrap;

            return label;
        }

        public static LineBreakMode GetLineBreakMode(string value)
        {
            switch (value)
            {
                case "NoWrap": return LineBreakMode.NoWrap;
                case "WordWrap": return LineBreakMode.WordWrap;
                case "CharacterWrap": return LineBreakMode.CharacterWrap;
                case "HeadTruncation": return LineBreakMode.HeadTruncation;
                case "TailTruncation": return LineBreakMode.TailTruncation;
                case "MiddleTruncation": return LineBreakMode.MiddleTruncation;
                default: return LineBreakMode.NoWrap;
            }
        }
    }
}