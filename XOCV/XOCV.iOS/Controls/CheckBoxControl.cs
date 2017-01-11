using UIKit;
using Foundation;
using CoreGraphics;

namespace XOCV.iOS
{
    [Register(nameof(CheckBoxControl))]

    public class CheckBoxControl : UIButton
    {
        public CheckBoxControl()
        {
            Initialize();
        }

        public CheckBoxControl(CGRect bounds) : base(bounds)
        {
            Initialize();
        }

        public bool Checked
        {
            set { Selected = value; }
            get { return Selected; }
        }

        void Initialize()
        {
            AdjustEdgeInsets();
            ApplyStyle();

            TouchUpInside += (sender, args) => Selected = !Selected;
        }

        void AdjustEdgeInsets()
        {
            const float Inset = 8f;
            HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
        }

        void ApplyStyle()
        {
            SetImage(UIImage.FromBundle("selected.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Selected);
            SetImage(UIImage.FromBundle("unselected.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
        }
    }
}