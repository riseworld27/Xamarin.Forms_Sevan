using System.Windows.Input;
using Xamarin.Forms;

namespace XOCV.Views
{
    public class TappedListView : ListView
    {
        public static BindableProperty ItemClickCommandProperty = 
            BindableProperty.Create<TappedListView, ICommand>
            (x => x.ItemClickCommand, null);

        public TappedListView()
        {
            this.ItemTapped += this.OnItemTapped;
        }

        public ICommand ItemClickCommand
        {
            get { return (ICommand)this.GetValue(ItemClickCommandProperty); }
            set { this.SetValue(ItemClickCommandProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && this.ItemClickCommand != null && this.ItemClickCommand.CanExecute(e))
            {
                this.ItemClickCommand.Execute(e.Item);
                this.SelectedItem = null;
            }
        }
    }
}