using System.Windows.Input;
using Xamarin.Forms;

namespace XOCV.Views
{
    public class CheckBox : View
    {
        public static readonly BindableProperty CheckedProperty = BindableProperty.Create("Checked", typeof(bool), typeof(CheckBox), false, BindingMode.TwoWay, null, OnCheckedPropertyChanged);

        public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(CheckBox), default(ICommand));

        public bool Checked
        {
            get { return this.GetValue<bool>(CheckedProperty); }
            set { 
                if (this.Checked != value)
                {
                    this.SetValue(CheckedProperty, value);
                    //this.CheckedChanged.Invoke(this, value);
                }
            }
        }

        public ICommand SelectionChangedCommand
        {
            get { return GetValue(SelectionChangedCommandProperty) as ICommand; }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        private static void OnCheckedPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var checkBox = (CheckBox)bindable;
            checkBox.Checked = (bool)newvalue;
            //SelectionChangedCommand.Execute(null);
        }

        //protected override void OnPropertyChanged(string propertyName = null)
        //{ 
        //	if (propertyName == SelectionChangedCommandProperty.PropertyName)
        //	if(SelectionChangedCommand!=null)
        //		SelectionChangedCommand.Execute(null);
        //	else
        //		base.OnPropertyChanged(propertyName);
        //}

        private T GetValue<T>(BindableProperty property)
        {
            return (T)GetValue(property);
        }
    }
}