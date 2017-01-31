using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace XOCV.Helpers.ControlHelpers.Base
{
    public class BaseControlHelper
    {
        public static LayoutOptions GetPositionOption(string value)
        {
            switch (value)
            {
                case "Center": return LayoutOptions.Center;
                case "CenterAndExpand": return LayoutOptions.CenterAndExpand;
                case "End": return LayoutOptions.End;
                case "EndAndExpand": return LayoutOptions.EndAndExpand;
                case "Fill": return LayoutOptions.Fill;
                case "FillAndExpand": return LayoutOptions.FillAndExpand;
                case "Start": return LayoutOptions.Start;
                case "StartAndExpand": return LayoutOptions.StartAndExpand;
                default: return LayoutOptions.Start;
            }
        }

        public static int GetNumericValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    var intValue = int.Parse(value);
                    return intValue;
                }
                catch (FormatException ex)
                {
                    Debug.WriteLine("Format exception! Cannot convert string to int! ( " + ex.Message + ")");
                }
            }
            return 12;
        }

        public static Color GetColor(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                System.Drawing.Color systemColor = System.Drawing.Color.FromName(value);
                var color = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                return color;
            }
            return Color.Black;
        }

        public static FontAttributes GetFontAttributes(string value)
        {
            switch (value)
            {
                case "None":
                    return FontAttributes.None;
                case "Bold":
                    return FontAttributes.Bold;
                case "Italic":
                    return FontAttributes.Italic;
                default: return FontAttributes.None;
            }
        }

        public static TextAlignment GetTextAlignmentPosition(string value)
        {
            switch (value)
            {
                case "Start": return TextAlignment.Start;
                case "Center": return TextAlignment.Center;
                case "End": return TextAlignment.End;
                default: return TextAlignment.Start;
            }
        }
    }
}