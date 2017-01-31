using System;
using System.IO;
using System.Threading.Tasks;
using ComponentPro.Net;
using ComponentPro.IO;
using System.Security.Cryptography.X509Certificates;
using ComponentPro.Security.Cryptography;
using ComponentPro.Security.Certificates;

namespace ComponentProSamples
{
	/// <summary>
	/// Defines operation results.
	/// </summary>
	public enum OperationResult
	{
		/// <summary>
		/// Operation is successful.
		/// </summary>
		Successful,
		/// <summary>
		/// Operation failed.
		/// </summary>
		Failed,
		/// <summary>
		///Connection closed and the caller needs to retry connecting to the server again.
		/// </summary>
		ConnectionClosed,
	}

	/// <summary>
	/// Represents a platform-independant class that contains the methods to work with the server.
	/// </summary>
	public class ClientLogic
	{
		public const int MaxPreviewLength = 16 * 1024;

		IConnectionInfo _connection;
		Sftp _client;
		IClientBrowserView _view;
		bool _busy;

		string _currentDir;

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentProSamples.ClientLogic"/> class.
		/// </summary>
		/// <param name="view">The IClientBrowserView class.</param>
		/// <param name="connectionInfo">Connection info.</param>
		public ClientLogic (IClientBrowserView view, IConnectionInfo connectionInfo)
		{
			_view = view;
			_connection = connectionInfo;
		}

		/// <summary>
		/// Asynchronously connect to the server.
		/// </summary>
		/// <returns><c>true</c> if connected; otherwise <c>false</c>.</returns>
		public async Task<bool> Connect()
		{
			SetBusy (true);
			_view.SetStatus ("Connecting to {0}:{1}...", _connection.Server, _connection.Port);
			
			try
			{
				_client = new Sftp ();
				_client.HostKeyVerifying += _client_HostKeyVerifying;
			
				await _client.ConnectAsync (_connection.Server, _connection.Port);
			}
			catch (Exception ex) 
			{
				_view.ShowMessage (string.Format ("Error while connecting to {0}: {1}.", _connection.Server, ex.Message), "Conection Failed", MessageBoxType.Error, true);
				SetBusy (false);
				return false;
			}

			try
			{
				await _client.AuthenticateAsync (_connection.Username, _connection.Password);
				_currentDir = await _client.GetCurrentDirectoryAsync();
			}
			catch (Exception ex) 
			{
				_view.ShowMessage (string.Format ("Error while authenticating user {0}: {1}.", _connection.Username, ex.Message), "Conection Failed", MessageBoxType.Error, true);
				SetBusy (false);
				return false;
			}

			await RefreshFolderList (_currentDir);

			SetBusy (false);
			return true;
		}

		void _client_HostKeyVerifying(object sender, HostKeyVerifyingEventArgs e)
        {           
			string info = "ssh-" + e.HostKeyAlgorithm + " " + e.HostKey;

			string[] options = new string[] { "Accept", "Reject" };
			int selected = _view.ShowAlert(info, "Do you want to accept this host key?", options);

			e.Accept = selected == 0;
        }

		/// <summary>
		/// Sets or clears the busy state.
		/// </summary>
		/// <param name="busy">Set to <c>true</c> if busy.</param>
		void SetBusy(bool busy)
		{
			if (_busy != busy) {
				_view.SetBusy (busy);
				_busy = busy;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the user is authenticated.
		/// </summary>
		/// <value><c>true</c> if the user is authenticated; otherwise, <c>false</c>.</value>
		public bool IsClientAuthenticated
		{
			get { return _client != null && _client.IsAuthenticated; }
		}

		/// <summary>
		/// Refreshs the folder list.
		/// </summary>
		/// <returns><c>true</c>, if folder list was refreshed, <c>false</c> if an error occurred and the session needs to be closed.</returns>
		/// <param name="folder">Folder name.</param>
		public async Task<OperationResult> RefreshFolderList(string folder)
		{
			bool dirChanged = false;
			FileInfoCollection list = null;
			OperationResult result = OperationResult.Successful;

			SetBusy (true);

			Exception error = null;

			try
			{
				if (_currentDir != folder)
				{
					await _client.SetCurrentDirectoryAsync(folder);
					dirChanged = true;
				}

				list = await _client.ListDirectoryAsync();
				_currentDir = folder;

				list.Sort(new AndComparer(
					new FileInfoComparer(FileInfoComparisonMethod.Type),
					new FileInfoComparer(FileInfoComparisonMethod.Name)));

				if (_currentDir != _client.DirectorySeparators[0].ToString())
				{
					list.Insert(0, _client.CreateFileInfo("..", System.IO.FileAttributes.Directory, -1, DateTime.MinValue));
				}

				_view.ListDirectory(list);
				_view.SetPathText(_currentDir);
			}
			catch (Exception ex) 
			{
				error = ex;
			}

			SetBusy (false);

			if (error != null) 
			{
				if (dirChanged) {
					// Try to set to the previous dir
					try {
						await _client.SetCurrentDirectoryAsync (_currentDir);
						list = await _client.ListDirectoryAsync ();
					} catch (Exception exInner) {
						_view.ShowMessage (string.Format ("Error while listing directory {0}: {1}.", folder, exInner.Message), "Listing Failed", MessageBoxType.Error, true);
						return OperationResult.ConnectionClosed;
					}
				}

				HandleException (error, string.Format ("Error while listing directory {0}. ", folder));
				result = OperationResult.Failed;
			}

			return result;
		}

		void HandleException(Exception ex, string additionalMessage)
		{
			if (ex.InnerException != null)
				ex = ex.InnerException;

			bool terminate = false;
			string msg = null;
			if (ex is ObjectDisposedException)
				terminate = true;
			else if (ex is IOException)
				msg = "Error while saving data to local disk. ";
			else 
			{
				NetworkException ne = ex as NetworkException;
				if (ne != null) 
				{
					if (ne.Status == NetworkExceptionStatus.ProtocolError)
						msg = "Error while transferring data between the client and server. ";
					else
						terminate = true;
				}
			}

			_view.ShowMessage (msg + additionalMessage + "Error: " + ex.Message, "FtpClient", MessageBoxType.Error, terminate);
		}

		/// <summary>
		/// Disconnects from the server.
		/// </summary>
		public async Task Disconnect()
		{
			if (_busy)
				return;

			if (_client != null) 
			{
				SetBusy (true);
				_view.SetStatus ("Disconnecting from {0}:{1}...", _connection.Server, _connection.Port);
				try
				{
					await _client.DisconnectAsync ();
				}
				catch (Exception ex) 
				{
					HandleException (ex, "Error while disconnecting from the server.");
				}
				SetBusy (false);
			}
		}

		/// <summary>
		/// Download the specified remote file to the local disk.
		/// </summary>
		/// <param name="remotePath">Remote path.</param>
		/// <param name="localPath">Local path.</param>
		public void Download(string remotePath, string localPath)
		{
			SetBusy (true);
			_view.SetStatus ("Downloading {0} -> {1}", remotePath, localPath);
			try
			{
				long len = _client.DownloadFile (remotePath, localPath);

				_view.ShowMessage (string.Format("{0} bytes downloaded.", len), remotePath, MessageBoxType.Information, false);
			}
			catch (Exception ex)
			{
				HandleException (ex, string.Format("Error while downloading file {0}.", remotePath));
			}
			finally
			{
				SetBusy (false);
			}
		}

		/// <summary>
		/// Download the specified remote file to a memory stream.
		/// </summary>
		/// <param name="remotePath">Remote path.</param>
		public async Task<MemoryStream> Download(string remotePath)
		{
			SetBusy (true);
			_view.SetStatus ("Downloading {0} -> {1}", remotePath, "memory");

			MemoryStream ms = new MemoryStream ();

			try
			{
				long len = await _client.DownloadFileAsync(remotePath, ms);

				_view.ShowMessage (string.Format("{0} bytes downloaded.", len), remotePath, MessageBoxType.Information, false);
			}
			catch (Exception ex)
			{
				ms.Close ();
				HandleException (ex, string.Format("Error while downloading file {0}.", remotePath));
			}
			finally
			{
				SetBusy (false);
			}

			return ms;
		}

		/// <summary>
		/// Downloads and views a portion of the remote file.
		/// </summary>
		/// <param name="remotePath">Remote path.</param>
		public async void ViewTextFile(string remotePath)
		{
			try
			{
				SetBusy (true);
				var ms = new MemoryStream ();
				_view.SetStatus ("Downloading content of {0}", remotePath);

				// We download the first 1000 bytes only.
				await _client.DownloadFileAsync (remotePath, ms, 0, 1000);

				// Get string representation of downloaded data
				string content = System.Text.Encoding.Default.GetString (ms.ToArray ());

				// Show file's content
				_view.ShowMessage (content, remotePath, MessageBoxType.Information, false);
			}
			catch (Exception ex) {
				HandleException (ex, string.Format("Error while downloading content of {0}."));
			}
			finally {
				SetBusy (false);
			}
		}

		/// <summary>
		/// Fired when user has clicked on a file item.
		/// </summary>
		/// <param name="file">File info object.</param>
		/// <param name="itemView">Item view.</param>
		public async void OnItemClicked(AbstractFileInfo file, object itemView)
		{
			if (_busy)
				return;

		Retry:
			if (file.IsDirectory) 
			{
				string path;

				if (file.Name == "..")
					path = _client.GetDirectoryName(_client.GetDirectoryName (file.FullName)); // Get the parent directory path.
				else
					path = file.FullName;

				await RefreshFolderList (path);
			}
			else if (file.IsSymlink) {
				file = await file.FileSystem.GetFileInfoAsync (file.SymlinkPath);
				goto Retry;
			} else
				_view.ShowContextMenu (file, itemView);
		}

        /// <summary>
        /// Returns all issues of the given certificate.
        /// </summary>
        private static string GetCertProblem(CertificateVerificationStatus status, int code, ref bool showAddTrusted)
        {
            switch (status)
            {
                case CertificateVerificationStatus.TimeNotValid:
                    return "Server's certificate has expired or is not valid yet.";

                case CertificateVerificationStatus.Revoked:
                    return "Server's certificate has been revoked.";

                case CertificateVerificationStatus.RootNotTrusted:
                    showAddTrusted = true;
                    return "Server's certificate was issued by an untrusted authority.";

                case CertificateVerificationStatus.IncompleteChain:
                    return "Server's certificate does not chain up to a trusted root authority.";

                case CertificateVerificationStatus.Malformed:
                    return "Server's certificate is malformed.";

                case CertificateVerificationStatus.CNNotMatch:
                    return "Server hostname does not match the certificate.";

                case CertificateVerificationStatus.UnknownError:
                    return string.Format("Error {0:x} encountered while validating server's certificate.", code);

                default:
                    return status.ToString();
            }
        }
	}
}

