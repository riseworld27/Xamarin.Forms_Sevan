using System;
using Xamarin.Forms;
namespace XOCV.Views
{
	public class AutoresizedEditor : Editor
	{
		bool sized = false;
		public double lineHeight = 0;

		public AutoresizedEditor()
		{
			this.TextChanged += (sender, e) => { this.InvalidateMeasure(); };
		}

		//protected override void OnSizeAllocated(double width, double height)
		//{
		//	if (!sized)
		//	{
		//		int count = Text.Split('\n').Length;
		//		lineHeight = (height / (count));
		//		sized = true;
		//	}
		//	base.OnSizeAllocated(width, height);
		//}
	}
}
