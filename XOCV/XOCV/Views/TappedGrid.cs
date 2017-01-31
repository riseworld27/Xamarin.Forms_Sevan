using System.Windows.Input;
using Xamarin.Forms;

namespace XOCV.Views
{
    public class TappedGrid : Grid
    {
        public static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create(nameof(TappedCommand),
                    typeof(ICommand),
                    typeof(TappedGrid),
                    default(ICommand));

        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }
    }
}