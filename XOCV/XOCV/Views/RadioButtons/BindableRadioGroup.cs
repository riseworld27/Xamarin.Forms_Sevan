using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XOCV.Helpers;

namespace XOCV.Views.RadioButtons
{
    public class BindableRadioGroup : StackLayout
    {
        public List<CustomRadioButton> rads;

        public BindableRadioGroup ()
        {
            rads = new List<CustomRadioButton> ();
            MultiSelectedIndexes = new ObservableCollection<string> ();
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindableRadioGroup, IEnumerable>
            (o => o.ItemsSource, default (IEnumerable), propertyChanged: OnItemsSourceChanged);


        public static BindableProperty SelectedIndexProperty =
            BindableProperty.Create<BindableRadioGroup, int>
            (o => o.SelectedIndex, default (int), BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

        public static BindableProperty MultiSelectedIndexesProperty =
            BindableProperty.Create<BindableRadioGroup, ObservableCollection<string>>
            (o => o.MultiSelectedIndexes, default (ObservableCollection<string>), BindingMode.TwoWay, propertyChanged: OnMultiSelectedIndexChanged);

        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue (ItemsSourceProperty); }
            set { SetValue (ItemsSourceProperty, value); }
        }

        public int SelectedIndex {
            get { return (int)GetValue (SelectedIndexProperty); }
            set { SetValue (SelectedIndexProperty, value); }
        }

        public ObservableCollection<string> MultiSelectedIndexes {
            get { return (ObservableCollection<string>)GetValue (MultiSelectedIndexesProperty); }
            set { SetValue (MultiSelectedIndexesProperty, value); }
        }

        public event EventHandler<int> CheckedChanged;

        private static void OnItemsSourceChanged (BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var radButtons = bindable as BindableRadioGroup;

            if (radButtons == null) return;

            radButtons.rads.Clear ();
            radButtons.Children.Clear ();
            if (newvalue != null) {
                int radIndex = 0;
                foreach (var item in newvalue) {
                    if (radIndex > 0) {
                        var rad = new CustomRadioButton ();
                        rad.Text = item.ToString ();
                        rad.Id = radIndex;

                        rad.CheckedChanged += radButtons.OnCheckedChanged;

                        radButtons.rads.Add (rad);

                        radButtons.Children.Add (rad);
                    }
                    radIndex++;
                }
            }
        }

        private void OnCheckedChanged (object sender, EventArgs<bool> e)
        {
            if (e.Value == false) return;

            var selectedRad = sender as CustomRadioButton;
            if (!MultiSelectedIndexes.Contains (selectedRad.Id.ToString ())) {
                if (MultiSelectedIndexes.Count == 0) {
                    MultiSelectedIndexes.Add (selectedRad.Id.ToString ());
                } else {
                    MultiSelectedIndexes.RemoveAt (0);
                    MultiSelectedIndexes.Add (selectedRad.Id.ToString ());
                }
            }
            SelectedIndex = selectedRad.Id;
            foreach (var rad in rads) {
                if (!selectedRad.Id.Equals (rad.Id)) {
                    rad.Checked = false;
                } else {
                    if (CheckedChanged != null)
                        CheckedChanged.Invoke (sender, rad.Id);
                }
            }
        }

        private void OnMultiCheckedChanged (object sender, EventArgs<bool> e)
        {
            if (e.Value == false) return;

            var selectedRad = sender as CustomRadioButton;
            SelectedIndex = selectedRad.Id;
            foreach (var rad in rads) {
                if (!selectedRad.Id.Equals (rad.Id)) {
                    rad.Checked = false;
                } else {
                    if (CheckedChanged != null)
                        CheckedChanged.Invoke (sender, rad.Id);
                }
            }
        }

        private static void OnSelectedIndexChanged (BindableObject bindable, int oldvalue, int newvalue)
        {
            if (newvalue == -1) return;

            var bindableRadioGroup = bindable as BindableRadioGroup;

            foreach (var rad in bindableRadioGroup.rads) {
                if (rad.Id == bindableRadioGroup.SelectedIndex) {
                    rad.Checked = true;
                }
            }
        }

        private static void OnMultiSelectedIndexChanged (BindableObject bindable, ObservableCollection<string> oldvalue, ObservableCollection<string> newvalue)
        {
            if (newvalue == null) return;

            var bindableRadioGroup = bindable as BindableRadioGroup;

            foreach (var rad in bindableRadioGroup.rads) {
                foreach (var index in bindableRadioGroup.MultiSelectedIndexes)
                    if (rad.Id == int.Parse (index)) {
                        rad.Checked = true;
                    }
            }
        }
    }
}