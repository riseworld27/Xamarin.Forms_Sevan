using System;
using System.Collections.Generic;

using Xamarin.Forms;
using XOCV.Pages.Base;

namespace XOCV.Pages
{
	public partial class SettingsPage : XOCVPage
	{
		public SettingsPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			NavigationPage.SetHasBackButton(this, false);
		}
	}
}
