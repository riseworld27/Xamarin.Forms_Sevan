using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ComponentPro.IO;
using ComponentPro.Net;

namespace ComponentProSamples
{
	/// <summary>
	/// This activity shows content of remote site. It makes it possible to browse the remote site, download and show files.
	/// </summary>
	[Activity(Label = "@string/ApplicationName", Icon = "@drawable/logo", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenLayout)]
	public class BrowserActivity : Activity, IClientBrowserView
	{
		IConnectionInfo _connectionInfo;
		ClientLogic _logic;
		string _lastStatus;

		/// <summary>
		/// Gets saved connection information from intent corresponding to remote site or from previously saved state.
		/// </summary>
		/// <param name="savedInstanceState">
		/// If the activity is being re-initialized after previously being shut down
		/// then it contains the data most recently supplied in <see cref="Activity.OnSaveInstanceState"/>.
		/// </param>
		/// <remarks>
		/// This is the first method in a activity lifecycle.
		/// </remarks>
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_connectionInfo = Intent.GetParcelableArrayListExtra (Util.ConnectionKey).Cast<IConnectionInfo>().First();
			_logic = new ClientLogic (this, _connectionInfo);
		}

		#region IView

		/// <summary>
		/// Sets the activity status.
		/// </summary>
		/// <param name="message">The message to show.</param>
		/// <param name="args">Format arguments.</param>
		void IView.SetStatus(string message, params object[] args)
		{
			_lastStatus = string.Format (message, args);
			if (_logic.IsClientAuthenticated)
				Toast.MakeText(this, string.Format(message, args), ToastLength.Short).Show();
		}

		/// <summary>
		/// Sets the busy state.
		/// </summary>
		/// <param name="busy"><c>true</c> if busy; otherwise <c>false</c>.</param>
		void IView.SetBusy(bool busy)
		{
			if (busy) 
			{
				if (!_logic.IsClientAuthenticated)
				{
					SetContentView (Resource.Layout.Progress);
					var description = FindViewById<TextView> (Resource.Id.lblDescription);
					description.Text = _lastStatus;
				}
			}
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

		void IClientBrowserView.SetPathText(string path)
		{
			var pathText = FindViewById<TextView>(Resource.Id.txtDirectoryPath);
			pathText.Text = path;
		}

		void IClientBrowserView.ListDirectory(FileInfoCollection list)
		{
			SetContentView (Resource.Layout.RemoteSite);

			var adapter = new FileInfoListAdapter (this, _logic, list);
			var dirListView = FindViewById<ListView>(Resource.Id.lstDirectoryContent);
			dirListView.Adapter = adapter;
			RegisterForContextMenu (dirListView);
		}

		void IClientBrowserView.ShowContextMenu(object itemView)
		{
			OpenContextMenu((View)itemView);
		}

		#endregion

		/// <summary>
		/// Connects when the activity is going to be visible.
		/// </summary>
		protected override async void OnStart()
		{
			base.OnStart();
			await _logic.Connect ();
		}

		/// <summary>
		/// Disconnects when the activity is no longer visible.
		/// </summary>
		protected override async void OnStop()
		{
			if (_logic != null)
				await _logic.Disconnect ();
			base.OnStop();
		}

		/// <summary>
		/// Disconnects and set result when Android Back button is pressed.
		/// </summary>
		public override async void OnBackPressed()
		{
			await _logic.Disconnect();
			SetResult(Result.Ok);
			base.OnBackPressed();
		}

		/// <summary>
		/// Gets called when the activity is going to be closed.
		/// It is used to save some state information for the future instances.
		/// </summary>
		/// <param name="outState">The object into which to store the information.</param>
		/// <remarks>
		/// The activity can be closed from different reasons when it is not active. Best example when this method comes handy is 
		/// when the user presses "home" button and then returns to the app. When the "home" button is pressed, this method is called.
		/// Then if the user returns to the application to this activity (e.g. through last running app menu), the outState is passed
		/// to the <see cref="OnCreate"/> method.
		/// </remarks>
		protected override void OnSaveInstanceState(Bundle outState)
		{

		}

		#region Options menu

		/// <summary>
		/// Creates Options menu. If client is authenticated shows the menu.
		/// </summary>
		/// <param name="menu">The menu to be displayed.</param>
		/// <returns>True.</returns>
		/// <remarks>
		/// This method is called only once, the first time the options menu is displayed.
		/// </remarks>
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu(menu);
			MenuInflater.Inflate(Resource.Layout.RemoteSiteMenu, menu);

			return true;
		}

		/// <summary>
		/// Adds disconnect item into options menu. Shows the menu if the client is authenticated.
		/// </summary>
		/// <param name="menu">The menu to be displayed.</param>
		/// <returns>True if client is authenticated.</returns>
		/// <remarks>
		/// This method is called every time the options menu is displayed.
		/// </remarks>
		public override bool OnPrepareOptionsMenu(IMenu menu)
		{
			if (_logic != null)
			{
				return _logic.IsClientAuthenticated;
			}

			return false;
		}

		/// <summary>
		/// Handles disconnect click.
		/// </summary>
		/// <param name="item">Clicked item (disconnect).</param>
		/// <returns>True if disconnect click was handled.</returns>
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.mniDisconnect:
				if (_logic != null)
					{						
						if (_logic.IsClientAuthenticated)
						{
							Task task = _logic.Disconnect ();
							task.Wait ();
						}
					}

					SetResult(Result.Ok);
					Finish();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		#endregion

		#region Context menu

		/// <summary>
		/// Creates context menu for a file system item.
		/// </summary>
		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
		{
			base.OnCreateContextMenu(menu, v, menuInfo);
			MenuInflater.Inflate(Resource.Layout.RemoteItemContextMenu, menu);

		}

		/// <summary>
		/// Handles context menu item click.
		/// </summary>
		/// <param name="item">Clicked menu item.</param>
		/// <returns>True if the click was handled.</returns>
		public override bool OnContextItemSelected(IMenuItem item)
		{

			var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;

			// retrieve the file name we stored within the view
			// (see RemoteSiteFolderAdapter.GetView method)
			string fileName = info.TargetView.GetTag(Resource.Id.ItemName).ToString();


			switch (item.ItemId)
			{
				case Resource.Id.mniViewText:
					Preview(fileName);
					return true;

				case Resource.Id.mniDownload:
					if (Android.OS.Environment.ExternalStorageState != Android.OS.Environment.MediaMounted)
					{
						Util.ShowMessage("No External Storage available.", "An SD card must be mounted in order to download a file.", this, null);				
						return true;
					}

					Download(fileName);

					return true;
			}

			return false;
		}

		/// <summary>
		/// Downloads a part of the file and displays it.
		/// </summary>
		/// <param name="fileName">Remote file name.</param>
		private void Preview(string fileName)
		{
			_logic.ViewTextFile (fileName);
		}

		/// <summary>
		/// Downloads file to local storage.
		/// </summary>
		/// <param name="fileName">Remote file name.</param>
		private void Download(string fileName)
		{
			string targetDirectory = DetermineTargetDirectory(fileName);
			string localPath = Path.Combine(targetDirectory, fileName);

			_logic.Download (fileName, localPath);
		}

		#endregion

		/// <summary>
		/// Finishes the activity.
		/// </summary>
		public override async void Finish()
		{
			if (_logic != null) {
				await _logic.Disconnect ();
				_logic = null;
			}

			base.Finish ();
		}

		/// <summary>
		/// Determines a target directory based on the file name extension.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <returns>Target directory.</returns>
		private static string DetermineTargetDirectory(string fileName)
		{
			string localDirectory;
			if (Util.IsPicture(fileName))
				localDirectory = Android.OS.Environment.DirectoryPictures;
			else if (Util.IsMusic(fileName))
				localDirectory = Android.OS.Environment.DirectoryMusic;
			else
				localDirectory = Android.OS.Environment.DirectoryDownloads;

			localDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(localDirectory).AbsolutePath;
			return localDirectory;
		}
	}
}