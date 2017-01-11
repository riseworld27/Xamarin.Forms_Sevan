using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XOCV.iOS.Renderers;
using XOCV.Views;

[assembly: ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]

namespace XOCV.iOS.Renderers
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
	{
		UIScrollView _native;

		CarouselLayout view;

		public CarouselLayoutRenderer()
		{
			PagingEnabled = true;
			ShowsHorizontalScrollIndicator = false;

			UILongPressGestureRecognizer longPress = new UILongPressGestureRecognizer((recognizer) =>
			{
				recognizer.MinimumPressDuration = 1.0;
				if (recognizer.State == UIGestureRecognizerState.Began)
				{
					view.HandleLongPress(view, new EventArgs());
				}
			});
			this.AddGestureRecognizer(longPress);

			//this.AddGestureRecognizer(new UILongPressGestureRecognizer((singlePress) =>
			//{
			//	singlePress.MinimumPressDuration = 0.000001;
			//	singlePress.NumberOfTouchesRequired = 1;
			//	singlePress.RequireGestureRecognizerToFail(longPress);
			//	if (singlePress.State == UIGestureRecognizerState.Began)
			//	{
			//		view.HandleSinglePress(view, new EventArgs());
			//	}
			//}));
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null) return;

			_native = (UIScrollView)NativeView;
			_native.Scrolled += NativeScrolled;
			e.NewElement.PropertyChanged += ElementPropertyChanged;

			if (e.NewElement != null)
				view = e.NewElement as CarouselLayout;
		}

		void NativeScrolled(object sender, EventArgs e)
		{
			var center = _native.ContentOffset.X + (_native.Bounds.Width / 2);
			((CarouselLayout)Element).SelectedIndex = ((int)center) / ((int)_native.Bounds.Width);
		}

		void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CarouselLayout.SelectedIndexProperty.PropertyName && !Dragging)
			{
				ScrollToSelection(false);
			}
		}

		void ScrollToSelection(bool animate)
		{
			if (Element == null) return;

			_native.SetContentOffset(new CoreGraphics.CGPoint
				(_native.Bounds.Width *
					Math.Max(0, ((CarouselLayout)Element).SelectedIndex),
					_native.ContentOffset.Y),
				animate);
		}

		public override void Draw(CoreGraphics.CGRect rect)
		{
			base.Draw(rect);
			ScrollToSelection(false);
		}
	}
}
