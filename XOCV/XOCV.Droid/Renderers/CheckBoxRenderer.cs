using System;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.V7.Widget;
using Xamarin.Forms;
using Color = Android.Graphics.Color;
using XOCV.Views;

[assembly: ExportRenderer(typeof(CheckBox), typeof(XOCV.Droid.Renderers.CheckBoxRenderer))]
namespace XOCV.Droid.Renderers
{
    public class CheckBoxRenderer : ViewRenderer<CheckBox, Android.Widget.CheckBox>
    {

        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var checkBox = new Android.Widget.CheckBox(this.Context);
                checkBox.CheckedChange += CheckBoxCheckedChange;

                this.SetNativeControl(checkBox);
            }

            if (e.NewElement != null)
            {
                Control.Checked = e.NewElement.Checked;
            }

            if (e.OldElement != null && Control != null)
            {
                Control.CheckedChange -= CheckBoxCheckedChange;
            }

        }

        
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                
                default:
                    //System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    break;
            }
        }


        void CheckBoxCheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

               
    }
}

