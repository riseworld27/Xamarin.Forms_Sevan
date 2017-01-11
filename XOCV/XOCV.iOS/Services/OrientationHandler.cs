using System;
using Foundation;
using UIKit;
using XOCV.Interfaces;

namespace XOCV.iOS.Services
{
	public class OrientationHandler : IOrientationHandler
	{
		public void ForceLandscape()
		{
			AppDelegate.orientation = UIInterfaceOrientationMask.LandscapeLeft;
		}

		public void ForceNormal()
		{
			AppDelegate.orientation = UIInterfaceOrientationMask.AllButUpsideDown;
		}

		public void ForcePortrait()
		{
			AppDelegate.orientation = UIInterfaceOrientationMask.Portrait;
		}
	}
}
