using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using UIKit;
using XOCV.Views;

[assembly: ExportRenderer(typeof(CheckBox), typeof(XOCV.iOS.CheckBoxRenderer))]
namespace XOCV.iOS
{
    public class CheckBoxRenderer : ViewRenderer<CheckBox, CheckBoxControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            BackgroundColor = Element.BackgroundColor.ToUIColor();

            if (Control == null)
            {
                var checkBox = new CheckBoxControl(Bounds);
                checkBox.TouchUpInside += (s, args) => Element.Checked = Control.Checked;

                SetNativeControl(checkBox);
            }

            Control.Frame = Frame;
            Control.Bounds = Bounds;
            Control.SetTitleColor(UIColor.FromRGB(50, 156, 0), UIControlState.Normal);
            Control.SetTitleColor(UIColor.FromRGB(50, 156, 0), UIControlState.Selected);

            Control.LineBreakMode = UILineBreakMode.CharacterWrap;
            Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
            Control.Checked = e.NewElement.Checked;
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                case "Element":
                    break;
                default:
                    //System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    return;
            }
        }
    }
}