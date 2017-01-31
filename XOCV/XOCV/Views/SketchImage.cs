using System;
using Xamarin.Forms;

namespace XOCV.Views
{
	public class SketchImage : Image
	{
		public static readonly BindableProperty CurrentLineColorProperty =
			BindableProperty.Create((SketchImage w) => w.CurrentLineColor, Color.Default);

		public Color CurrentLineColor
		{
			get
			{
				return (Color)GetValue(CurrentLineColorProperty);
			}
			set
			{
				SetValue(CurrentLineColorProperty, value);
			}
		}
	}
}
