using Xamarin.Forms;

namespace XOCV.Views
{
    public class CustomEntry : Entry
    {
        public Color BorderColor { get; set; } = Color.FromHex ("#D3D3D3");
        public double BorderWidth { get; set; } = 0.4;
        public double CornerRadius { get; set; } = 5;
    }
}