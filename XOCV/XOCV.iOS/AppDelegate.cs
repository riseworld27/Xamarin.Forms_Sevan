using Foundation;
using UIKit;
using FFImageLoading;
using FFImageLoading.Forms.Touch;
using XLabs.Forms;
using Xamarin.Forms;
using System.Linq;
using XOCV.Pages;
using System;

namespace XOCV.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {

		public static UIInterfaceOrientationMask orientation;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
			ComponentPro.Licensing.Common.LicenseManager.SetLicenseKey("669A85ABDA76DB56D60336C89D5CAA6D813ED9B6D456C00FF452B7E59A689719");
			Forms.Init ();
            LoadApplication (new App ());
            ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer.Init ();

            return base.FinishedLaunching (app, options);
        }

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
		{
			return orientation;
		}
    }
}
