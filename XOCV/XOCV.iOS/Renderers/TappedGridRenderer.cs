using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XOCV.iOS.Renderers;
using XOCV.Views;

[assembly: ExportRenderer(typeof(TappedGrid), typeof(TappedGridRenderer))]
namespace XOCV.iOS.Renderers
{
    public class TappedGridRenderer : ViewRenderer
    {
        UITapGestureRecognizer tapGesturesRecognizer;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            tapGesturesRecognizer = new UITapGestureRecognizer(() => ((TappedGrid)Element).TappedCommand.Execute(Element.BindingContext));

            if (e.NewElement == null)
            {
                this.RemoveGestureRecognizer(tapGesturesRecognizer);
            }

            if (e.OldElement == null)
            {
                this.AddGestureRecognizer(tapGesturesRecognizer);
            }
        }
    }
}