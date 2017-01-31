using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XOCV.Droid.Renderers;
using XOCV.Views;

[assembly: ExportRenderer (typeof (CustomButton), typeof (CustomButtomRenderer))]
namespace XOCV.Droid.Renderers
{
    public class CustomButtomRenderer : ButtonRenderer
    {

        protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged (e);
            if (Control != null) {
                Control.SetPadding (0, 0, 0, 0);

                setAllCaps ();

            }
        }

        private void setAllCaps ()
        {
            var control = Control as Android.Widget.Button;
            control.SetAllCaps (false);
        }
    }
}
