using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MonoTouch.Dialog;
using Foundation;
using UIKit;

using ComponentPro.Net;
using ComponentPro.IO;
using ComponentPro.Security.Certificates;

namespace ComponentProSamples
{
	/// <summary>
	/// Class representing a remote connection GUI.
	/// </summary>
	public class ConnectView : DialogViewController, IView
	{
        // main container for the application
		private AppWindow _window;

		private EntryElement _siteName;
		private EntryElement _serverName;
		private EntryElement _serverPort;
		private EntryElement _username;
		private EntryElement _password;

		private UIButton _connectButton;
		private Section _propertiesSection;

		public string ServerName
		{
			get { return _serverName.Value; }
		}

		public int ServerPort
		{
			get
			{
				ushort value;
				if (!ushort.TryParse(_serverPort.Value, out value))
					return -1;
				
				return value;
			}
			private set
			{
				_serverPort.Value = value.ToString();
			}
		}

		public string Password
		{
			get { return _password.Value; }
		}

		public string UserName
		{
			get { return _username.Value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectView"/> class.
		/// </summary>
		/// <param name="siteName">The remote site name.</param>
		/// <param name="navigation">AppWindow.</param>
		public ConnectView(AppWindow window, IConnectionInfo info):
		base(new RootElement("Connection Info"), true)
		{
			// Initialize controller's elements
			_window = window;

			_siteName = new EntryElement("Description: ", "Enter site description", info.Name);
			_siteName.AutocorrectionType = UITextAutocorrectionType.No;
			_siteName.Changed += (sender, e) => 
			{
				Root.Caption = ((EntryElement)sender).Value; 
				this.ReloadData();
			};

			_serverName = new EntryElement("Hostname: ", "Enter server address", info.Server);
			Util.DisableAutoCorrectionAndAutoCapitalization(_serverName);

			string defaultPort = info.Port.ToString();
			_serverPort = new EntryElement("Port: ", "Enter port", defaultPort);
			_serverPort.KeyboardType = UIKit.UIKeyboardType.NumberPad;

			_username = new EntryElement("Username: ", "Enter username", info.Username);
			Util.DisableAutoCorrectionAndAutoCapitalization(_username);

			_password = new EntryElement("Password: ", "Enter password", info.Password, true);
			Util.DisableAutoCorrectionAndAutoCapitalization(_password);

			_connectButton = UIButton.FromType (UIButtonType.System);
            _connectButton.SetTitle("Connect", UIControlState.Normal);
			_connectButton.Frame = new CoreGraphics.CGRect ((_window.Screen.Bounds.Width - 140) / 2, 250, 140, 40);
			_connectButton.TouchUpInside += (s, e) => Connect();


			_propertiesSection = new Section()
			{
				_siteName,
				_serverName,
				_serverPort,
				_username,
				_password
			};

			Root.Add(_propertiesSection);

			View.AddSubview (_connectButton);
			//Section buttonSection = new Section ();
			//buttonSection.Add (_connectButton);
			//Root.Add(buttonSection);
		}

		/// <summary>
		/// Connect to the server and display home directory contents.
		/// </summary>
		private void Connect()
		{
            IConnectionInfo info = new ConnectionInfo();
            info.Name = _siteName.Value;
            info.Server = _serverName.Value;
            info.Port = ServerPort;
            info.Username = _username.Value;
            info.Password = _password.Value;

			// Check the connection parameters.
			if (!SiteManagerLogic.CheckConnectionParameters(this, info))
                return;

            // Create Folder View which represents the directory.
			FolderView folder = new FolderView(_window, info, "");

            // Show Folder View to the user
            _window.PushViewController(folder, true);
		}

        #region IView

        /// <summary>
        /// Sets the activity status.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="args">Format arguments.</param>
        void IView.SetStatus(string message, params object[] args)
        {
            
        }

        int IView.ShowAlert(string message, string title, params string[] options)
        {
            var dialog = new UIAlertView(title, message, null, "Cancel", options);
            return Util.ShowDialog(dialog);
        }

        /// <summary>
        /// Sets the busy state.
        /// </summary>
        /// <param name="busy"><c>true</c> if busy; otherwise <c>false</c>.</param>
        void IView.SetBusy(bool busy)
        {
			_window.Busy = busy;
        }

        void IView.ShowMessage(string message, string title, MessageBoxType type, bool terminate)
        {
            Util.ShowMessage(title, message);

            if (terminate)
                _window.PopToViewController(this, true);
        }

        #endregion
	}
}
