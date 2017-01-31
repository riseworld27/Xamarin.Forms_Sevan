using System;
using Xamarin.Forms;
using XOCV.Views;
using XOCV.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AutoresizedEditor), typeof(AutoresizedEditorRenderer))]
namespace XOCV.iOS.Renderers
{
	public class AutoresizedEditorRenderer : EditorRenderer
	{
		public AutoresizedEditorRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				Control.ScrollEnabled = false;

				var layer = Control.Layer;
				layer.BorderColor = Color.FromHex("#b1e3ac").ToCGColor();
				layer.BorderWidth = 1f;
				layer.CornerRadius = 5;
			}
		}
	}
}
