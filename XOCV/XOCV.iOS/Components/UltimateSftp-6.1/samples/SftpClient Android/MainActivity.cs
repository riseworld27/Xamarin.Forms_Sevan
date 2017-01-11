using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content.PM;

namespace ComponentProSamples
{
	/// <summary>
	/// Main activity of the app. Presents a list of sites.
	/// </summary>
	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/logo", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout)]
	public class MainActivity : Activity, IView
	{
		SiteManagerLogic _siteManager;

		/// <summary>
		/// Prepares the main activity.
		/// </summary>
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			_siteManager = new SiteManagerLogic (this);

			// Load site list.
			LoadSiteList ();

			var connectButton = (Button)FindViewById(Resource.Id.btnConnectTop);
			connectButton.Click += Connect_Clicked;

			var sitesSpinner = (Spinner)FindViewById(Resource.Id.spSites);
			sitesSpinner.ItemSelected += spinner_ItemSelected;
		}

		void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner s = (Spinner)sender;

			IConnectionInfo info = _siteManager.GetConnectionInfoList () [s.SelectedItemPosition];

			var name = (EditText)FindViewById (Resource.Id.txtSiteName);
			var server = (EditText)FindViewById (Resource.Id.txtServerName);
			var port = (EditText)FindViewById (Resource.Id.txtPort);
			var username = (EditText)FindViewById (Resource.Id.txtUserName);
			var pass = (EditText)FindViewById (Resource.Id.txtPassword);

			name.Text = info.Name;
			server.Text = info.Server;
			port.Text = info.Port.ToString ();
			username.Text = info.Username;
			pass.Text = info.Password;
		}

		#region IView

		void IView.SetStatus(string message, params object[] args)
		{
		}

		void IView.SetBusy(bool busy)
		{
		}

		void IView.ShowMessage(string message, string title, MessageBoxType type, bool terminate)
		{
			if (terminate)
				Util.ShowMessage (title, message, this, (sender, e) => {
					SetResult(Result.Ok);
					Finish();
				}
				);
			else
				Util.ShowMessage (title, message, this, null);
		}

		#endregion

		void Connect_Clicked(object sender, EventArgs e)
		{
			ConnectionInfo info = GetConnectionInfo ();

			if (_siteManager.CheckConnectionParameters (info)) 
			{
				var connectionIntent = new Intent (this, typeof(BrowserActivity));

				connectionIntent.PutParcelableArrayListExtra (Util.ConnectionKey, info.ToList ());

				StartActivity (connectionIntent);
			}
		}

		ConnectionInfo GetConnectionInfo()
		{
			ConnectionInfo s = new ConnectionInfo ();
			s.Server = ((EditText)FindViewById(Resource.Id.txtServerName)).Text;
			s.Port = int.Parse(((EditText)FindViewById(Resource.Id.txtPort)).Text);
			s.Username = ((EditText)FindViewById(Resource.Id.txtUserName)).Text;
			s.Password = ((EditText)FindViewById(Resource.Id.txtPassword)).Text;

			return s;
		}

		void LoadSiteList()
		{
			_siteManager.Load ();

			IConnectionInfo[] list = _siteManager.GetConnectionInfoList ();
			string[] names = new string[list.Length];
			for (int i = 0; i < list.Length; i++) {
				names [i] = list [i].Name;
			}

			var sitesSpinner = (Spinner)FindViewById(Resource.Id.spSites);
			ArrayAdapter adapter = new ArrayAdapter (this, Android.Resource.Layout.SimpleSpinnerDropDownItem, names);
			sitesSpinner.Adapter = adapter;
		}
	}
}