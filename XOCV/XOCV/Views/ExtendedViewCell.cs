using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XOCV.Views
{
    public class ExtendedViewCell : ViewCell
    {
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(ExtendedViewCell), Color.Transparent);

        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create("SeparatorColor", typeof(Color), typeof(ExtendedViewCell), Color.FromRgba(199, 199, 204, 255));

        public static readonly BindableProperty SeparatorPaddingProperty = BindableProperty.Create("SeparatorPadding", typeof(Thickness), typeof(ExtendedViewCell), default(Thickness));

        public static readonly BindableProperty ShowSeparatorProperty = BindableProperty.Create("ShowSeparator", typeof(bool), typeof(ExtendedViewCell), true);

        public static readonly BindableProperty ShowDisclosureProperty = BindableProperty.Create("ShowDisclosure", typeof(bool), typeof(ExtendedViewCell), false);

        public static readonly BindableProperty DisclosureImageProperty = BindableProperty.Create("DisclosureImage", typeof(string), typeof(ExtendedViewCell), string.Empty);

        public static readonly BindableProperty HighlightSelectionProperty = BindableProperty.Create("HighlightSelection", typeof(bool), typeof(ExtendedViewCell), true);

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        public Thickness SeparatorPadding
        {
            get { return (Thickness)GetValue(SeparatorPaddingProperty); }
            set { SetValue(SeparatorPaddingProperty, value); }
        }

        public bool ShowSeparator
        {
            get { return (bool)GetValue(ShowSeparatorProperty); }
            set { SetValue(ShowSeparatorProperty, value); }
        }

        public bool ShowDisclosure
        {
            get { return (bool)GetValue(ShowDisclosureProperty); }
            set { SetValue(ShowDisclosureProperty, value); }
        }

        public string DisclosureImage
        {
            get { return (string)GetValue(DisclosureImageProperty); }
            set { SetValue(DisclosureImageProperty, value); }
        }

        public bool HighlightSelection
        {
            get { return (bool)GetValue(HighlightSelectionProperty); }
            set { SetValue(HighlightSelectionProperty, value); }
        }
    }
}
