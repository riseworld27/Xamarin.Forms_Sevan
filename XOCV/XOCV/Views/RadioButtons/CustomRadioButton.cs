using System;
using Xamarin.Forms;
using XOCV.Extensions;
using XOCV.Helpers;

namespace XOCV.Views.RadioButtons
{
    public class CustomRadioButton : View
    {
        public static readonly BindableProperty CheckedProperty = BindableProperty.Create<CustomRadioButton, bool>(p => p.Checked, false);

        /// <summary>
        /// The default text property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create<CustomRadioButton, string>(p => p.Text, string.Empty);

        /// <summary>
        /// The checked changed event.
        /// </summary>
        public EventHandler<EventArgs<bool>> CheckedChanged;


        /// <summary>
        /// Identifies the TextColor bindable property.
        /// </summary>
        /// 
        /// <remarks/>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create<CustomRadioButton, Color>(p => p.TextColor, Color.Black);

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public bool Checked
        {
            get { return this.GetValue<bool>(CheckedProperty); }
            set {
                this.SetValue(CheckedProperty, value);
                var eventHandler = this.CheckedChanged;
                if (eventHandler != null)
                {
                    eventHandler.Invoke(this, value);
                }
            }
        }

        public string Text
        {
            get { return this.GetValue<string>(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public Color TextColor
        {
            get { return this.GetValue<Color>(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        public int Id { get; set; }
    }
}