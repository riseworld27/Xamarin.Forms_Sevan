using System;
using System.IO;

using MonoTouch.Dialog;
using UIKit;
using Foundation;

using ComponentPro.Net;
using ComponentPro.IO;

namespace ComponentProSamples
{
	/// <summary>
	/// Custom DialogViewController representing the directory and its contents.
	/// </summary>
	public class FolderView : DialogViewController, IClientBrowserView
	{
        AppWindow _window;

        ClientLogic _logic;

        // icons for files, directories, etc.
        static UIImage _imgFile = UIImage.FromFile("file.png");
        static UIImage _imgLink = UIImage.FromFile("link.png");

        static UIImage _imgFolder = UIImage.FromFile("folder.png");
        static UIImage _imgUp = UIImage.FromFile("up.png");

        public FolderView(AppWindow window, IConnectionInfo connection, string path)
			: base(CreateRootElemet(path), true)
        {
			_window = window;
            _logic = new ClientLogic(this, connection);

			var disconnectButton = new UIBarButtonItem ();
			disconnectButton.Title = "Close";
			disconnectButton.Clicked += Disconnect;

			//NavigationItem.LeftBarButtonItem.Clicked += Disconnect;
			NavigationItem.RightBarButtonItem = disconnectButton;
        }

        /// <summary>
        /// Creates the RootElement container which will later be used to show the item listing.
        /// </summary>
        /// <returns>The root elemet.</returns>
        /// <param name="listingPath">Listing path.</param>
		private static RootElement CreateRootElemet(string listingPath)
        {
            string displayPath = GetDisplayPath(listingPath);
			return new RootElement(displayPath);
		}

        /// <summary>
        /// Determine the path to display based on the device.
        /// </summary>
        /// <returns>The display path.</returns>
        /// <param name="listingPath">Listing path.</param>
        private static string GetDisplayPath(string listingPath)
        {
			if (listingPath.Length > 1 && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
                return System.IO.Path.GetFileName(listingPath); //only display the current directory on iPhone
            else
                return listingPath; // display the whole path
        }

		public override async void ViewDidLoad()
        {
			base.ViewDidLoad();

			if (!await _logic.Connect())
				_window.PopViewController (true);
        }

		/// <summary>
		/// Disconnects from the server.
		/// </summary>
		async void Disconnect(object sender, EventArgs e)
		{
			await _logic.Disconnect();
			_window.PopViewController (true);
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

        /// <summary>
        /// Sets the busy state.
        /// </summary>
        /// <param name="busy"><c>true</c> if busy; otherwise <c>false</c>.</param>
        void IView.SetBusy(bool busy)
        {
			_window.Busy = busy;
        }

        int IView.ShowAlert(string message, string title, params string[] options)
		{
			string[] options2 = new string[options.Length - 1];
			for (int i = 0; i < options2.Length; i++)
				options2 [i] = options [i];

			var dialog = new UIAlertView (title, message, null, options[options.Length - 1], options2);
			int selected = Util.ShowDialog (dialog);

			return options.Length - selected - 1;
		}

        void IView.ShowMessage(string message, string title, MessageBoxType type, bool terminate)
        {
            Util.ShowMessage(title, message);

            if (terminate)
                _window.PopToViewController(this, true);
        }

        void IClientBrowserView.SetPathText(string path)
        {
			NavigationItem.Title = GetDisplayPath(path);
        }

        void IClientBrowserView.ListDirectory(FileInfoCollection list)
        {
            Section itemList = new Section();

			// Loop through the list to add files.
            for (int i = 0; i < list.Count; i++)
            {
                AbstractFileInfo item = list[i];

                StringElement itemElement;
                if (item.IsDirectory)
					itemElement = new ImageStringElement(item.Name, item.Name == ".." ? _imgUp : _imgFolder);
                else if (item.IsFile)
                    itemElement = new ImageStringElement(item.Name, _imgFile);
                else if (item.IsSymlink)
                    itemElement = new ImageStringElement(item.Name, _imgLink);
                else
                    itemElement = new StringElement(item.Name);

                itemElement.Tapped += () =>
                {
					// Invoke the ClientLogic's ItemClick handler.
                    _logic.OnItemClicked(item, null);
                };

				// Add the item to the list.
                itemList.Add(itemElement);
            }

			Root.Clear ();
			// The file list will be shown.
            Root.Add(itemList);
        }

		/// <summary>
		/// Determines whether extension is a supported image extension.
		/// </summary>
		private static bool IsImageExtension(string ext)
		{
			return 
				   ext == ".tiff" || ext == ".tif" 
				|| ext == ".jpg"  || ext == ".jpeg"
				|| ext == ".gif" || ext == ".png"
				|| ext == ".bmp" || ext == ".BMPf" 
				|| ext == ".ico"
				|| ext == ".cur" || ext == ".xbm";
		}

		/// <summary>
		/// Saves the specified remote file to photos.
		/// </summary>
		private async void SaveToPhotos(string remoteFile)
		{
			MemoryStream ms = null;

			try
			{
				// Download to a memory stream.
				ms = await _logic.Download(remoteFile);

				Util.SaveToPhotos(NSData.FromArray(ms.ToArray()));
			}
			finally 
			{
				if (ms != null)
					ms.Close ();
			}
		}

		async void IClientBrowserView.ShowContextMenu(AbstractFileInfo item, object itemView)
        {
			string remoteFile = item.FullName;
			long length = item.Length;
			bool isImage = IsImageExtension(Path.GetExtension(remoteFile));

			// Files with size that is greater than the defined length wont have the preview option.
			bool enablePreview = length <= ClientLogic.MaxPreviewLength;

			string operationLabel = (isImage ? "Save to Photos" : "Download file");
			string[] otherButtons;

			if (enablePreview)
				otherButtons = new string[] { operationLabel, "Preview File" };
			else
				otherButtons = new string[] { operationLabel };

			var dialog = new UIAlertView("Please choose option for file ", string.Format("{0}\n({1})", item.Name, Util.FormatSize(length)), null, "Cancel", otherButtons);
			int action = await Util.ShowDialogAsync (dialog);

			switch (action)
			{
				case 1: // Download file.
					if (!isImage) // If file is not an image, download it to App's documents.
					{
						string localName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.GetFileName(remoteFile));
						_logic.Download(item.FullName, localName);
					}
					else // Otherwise download it to the Photos
					{
						SaveToPhotos(remoteFile);
					}
					break;
				case 2: // Preview file content
					_logic.ViewTextFile(remoteFile);
					break;
			}
        }

        #endregion
	}
}
