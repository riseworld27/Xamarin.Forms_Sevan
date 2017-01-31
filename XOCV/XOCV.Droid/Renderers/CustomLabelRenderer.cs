using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XOCV.Droid.Renderers;
using XOCV.Views;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace XOCV.Droid.Renderers
{
	public class CustomLabelRenderer: LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				SetTextColor();
			}
		}

		private void SetTextColor()
		{
			if (Element.TextColor == Xamarin.Forms.Color.Default)
			{
				Android.Graphics.Color andrColor = Android.Graphics.Color.Black;
				Control.SetTextColor(andrColor);
			}
		}
	}
}
