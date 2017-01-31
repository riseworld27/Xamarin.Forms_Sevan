using Xamarin.Forms;
using Android.Graphics;
using XOCV.Views;
using XOCV.Droid.Renderers;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (CustomEntry), typeof (CustomEntryRenderer))]
namespace XOCV.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer ()
        {
            SetWillNotDraw (false);
        }

        protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged (e);

            if (Control != null) {
                Control.SetPadding (8, 5, 0, 3);
                Control.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
            }
        }

        public override void Draw (Canvas canvas)
        {

            var entry = Element as CustomEntry;

            var width = (float)entry.BorderWidth * Resources.DisplayMetrics.Density;
            var radius = (float)entry.CornerRadius * Resources.DisplayMetrics.Density;

            var rect = new RectF (canvas.ClipBounds);

            rect.Inset (width, width);
            FillRect (canvas, rect, radius);
            DrawStroke (canvas, rect, radius, width);

            base.Draw (canvas);

        }

        void FillRect (Canvas canvas, RectF rect, float radius)
        {
            var paint = new Paint {

                Color = Android.Graphics.Color.White,
                AntiAlias = true,
            };
            canvas.DrawRoundRect (rect, radius, radius, paint);
        }

        void DrawStroke (Canvas canvas, RectF rect, float radius, float width)
        {
            var paint = new Paint {
                Color = (Element as CustomEntry).BorderColor.ToAndroid (),
                StrokeWidth = width,
                AntiAlias = false,
            };
            paint.SetStyle (Paint.Style.Stroke);
            canvas.DrawRoundRect (rect, radius, radius, paint);
        }
    }
}
