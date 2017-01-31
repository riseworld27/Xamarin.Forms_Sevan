using System;

using MonoTouch.Dialog;
using UIKit;

namespace ComponentProSamples
{
	/// <summary>
	/// Represents a custom Main Window class that contains utility methods to push/pop 
	/// view controllers to/from the navigation stack trace.
	/// </summary>
	public class AppWindow : UIWindow
	{
		private readonly UINavigationController _appNavigation;

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentProSamples.AppWindow"/> class.
		/// </summary>
		public AppWindow() : base(UIScreen.MainScreen.Bounds)
		{
			// Initialize the app's navigation controller.
			_appNavigation = new AppNavigationController();
			RootViewController = _appNavigation;
		}

		/// <summary>
		/// Starts the specified main ViewController.
		/// </summary>
		/// <param name="mainViewController">The main view controller to start.</param>
		public void Start(UIViewController mainViewController)
		{
			// push the controller onto the navigation stack trace
			_appNavigation.PushViewController(mainViewController, false);

			this.MakeKeyAndVisible();
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ComponentProSamples.AppWindow"/> is busy.
		/// </summary>
		/// <value><c>true</c> if busy; otherwise, <c>false</c>.</value>
		public bool Busy
		{
			get 
			{
				return UIApplication.SharedApplication.NetworkActivityIndicatorVisible;
			}
			set 
			{
				UserInteractionEnabled = !value;
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = value;
			}
		}

		/// <summary>
		/// Pushes UIViewController to the navigation stack.
		/// </summary>
		public void PushViewController(UIViewController viewController, bool animated)
		{
			_appNavigation.PushViewController(viewController, animated);
		}

		/// <summary>
		/// Pops UIViewController from the navigation stack.
		/// </summary>
		public UIViewController PopViewController(bool animated)
		{
			return _appNavigation.PopViewController(animated);
		}

		/// <summary>
		/// Pops from the navigation stack untill viewController is encountered.
		/// </summary>
		public void PopToViewController(UIViewController viewController, bool animated)
		{
			_appNavigation.PopToViewController(viewController, animated);
		}

		private class AppNavigationController : UINavigationController
		{
			public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
			{
				base.WillRotate(toInterfaceOrientation, duration);

				ViewControllers[0].WillRotate(toInterfaceOrientation, duration);
			}

			public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
			{
				base.DidRotate(fromInterfaceOrientation);

				ViewControllers[0].DidRotate(fromInterfaceOrientation);
			}
		}
	}
}
