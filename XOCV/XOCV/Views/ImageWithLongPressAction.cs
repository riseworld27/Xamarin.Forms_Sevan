using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XOCV.Views
{
    public class ImageWithLongPressAction : StackLayout
    {
        public event EventHandler LongPressActivated;

		public void HandleLongPress(object sender, EventArgs e)
		{
			if (LongPressActivated != null)
			{
				LongPressActivated(this, new EventArgs());
			}
		}

		public event EventHandler SinglePressActivated;

		public void HandleSinglePress(object sender, EventArgs e)
		{
			if (LongPressActivated != null)
			{
				SinglePressActivated(this, new EventArgs());
			}
		}
    }
}