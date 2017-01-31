using Foundation;
using UIKit;

namespace XOCV.iOS.Extensions
{
    public static class StringExtensions
    {
        public static float StringHeight(this string text, UIFont font, float width)
        {
            var nativeString = new NSString(text);

            var rect = nativeString.GetBoundingRect(
                new System.Drawing.SizeF(width, float.MaxValue),
                NSStringDrawingOptions.UsesLineFragmentOrigin,
                new UIStringAttributes() { Font = font },
                null);

            return (float)rect.Height;
        }
    }
}