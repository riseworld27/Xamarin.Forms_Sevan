using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XOCV.Views;

[assembly: ExportRenderer(typeof(XOCV.Views.ImageWithLongPressAction), typeof(XOCV.iOS.Renderers.ImageWithLongPressActionRenderer))]
namespace XOCV.iOS.Renderers
{
    public class ImageWithLongPressActionRenderer : ViewRenderer
    {
        ImageWithLongPressAction view;

		public ImageWithLongPressActionRenderer()
		{
			UILongPressGestureRecognizer longPress = new UILongPressGestureRecognizer((recognizer) =>
			{
				recognizer.MinimumPressDuration = 1.0;
				if (recognizer.State == UIGestureRecognizerState.Began)
				{
					view.HandleLongPress(view, new EventArgs());
				}
			});
			this.AddGestureRecognizer(longPress);

			this.AddGestureRecognizer(new UILongPressGestureRecognizer((singlePress) =>
			{
				singlePress.MinimumPressDuration = 0.000001;
				singlePress.NumberOfTouchesRequired = 1;
				singlePress.RequireGestureRecognizerToFail(longPress);
				if (singlePress.State == UIGestureRecognizerState.Began){
					view.HandleSinglePress(view, new EventArgs());
				}
			}));
		}

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
				view = e.NewElement as ImageWithLongPressAction;
		}
    }
}