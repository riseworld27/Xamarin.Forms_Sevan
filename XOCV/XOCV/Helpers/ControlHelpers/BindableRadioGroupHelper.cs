using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Helpers.ControlHelpers.Base;
using XOCV.Models.ResponseModels;
using XOCV.Views.RadioButtons;

namespace XOCV.Helpers.ControlHelpers
{
    [ImplementPropertyChanged]
    public class BindableRadioGroupHelper : BaseControlHelper
    {
        public static BindableRadioGroup SetRadioGroupProperties (Item model)
        {
            BindableRadioGroup radioGroup = new BindableRadioGroup ();

            radioGroup.ItemsSource = model.RadioButtonItemsSource;
            radioGroup.Padding = new Thickness (20, 0, 0, 0);

            return radioGroup;
        }

        public static BindableMultipleSelectionRagioGroup SetMultipleSelectionRadioGroupProperties (Item model)
        {
            BindableMultipleSelectionRagioGroup radioGroup = new BindableMultipleSelectionRagioGroup ();

            radioGroup.ItemsSource = model.RadioButtonItemsSource;
            radioGroup.Padding = new Thickness (20, 0, 0, 0);

            return radioGroup;
        }

        public static BindableHorizontalRadioGroup SetBindableHorizontalRadioGroupProperties(Item model)
        {
            BindableHorizontalRadioGroup radioGroup = new BindableHorizontalRadioGroup();

            radioGroup.ItemsSource = model.RadioButtonItemsSource;
            radioGroup.Padding = new Thickness(20, 0, 0, 0);

            return radioGroup;
        }
    }
}