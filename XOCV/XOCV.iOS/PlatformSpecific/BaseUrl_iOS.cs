using Xamarin.Forms;
using Foundation;
using XOCV.iOS.PlatformSpecific;
using XOCV.Interfaces;

[assembly: Dependency(typeof(BaseUrl_iOS))]

namespace XOCV.iOS.PlatformSpecific
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}