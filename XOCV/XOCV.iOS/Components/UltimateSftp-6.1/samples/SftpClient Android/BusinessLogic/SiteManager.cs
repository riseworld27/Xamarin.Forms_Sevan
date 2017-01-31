using System;
using System.Collections.Generic;

namespace ComponentProSamples
{
	/// <summary>
	/// Site manager logic for the Main activity.
	/// </summary>
	public class SiteManagerLogic
	{
		List<IConnectionInfo> list;
		IView _view;

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentProSamples.SiteManagerLogic"/> class.
		/// </summary>
		/// <param name="view">View.</param>
		public SiteManagerLogic (IView view)
		{
			_view = view;
		}

		/// <summary>
		/// Checks the connection parameters.
		/// </summary>
		/// <returns><c>true</c>, if connection parameters are valid, <c>false</c> otherwise.</returns>
		/// <param name="connection">The connection information.</param>
		public bool CheckConnectionParameters(IConnectionInfo connection)
		{
			// Check parameters.
			if (string.IsNullOrEmpty(connection.Server))
			{
				_view.ShowMessage("Please specify server to connect to.", "Server address cannot be empty.", MessageBoxType.Error, false);
				return false;
			}

			if (connection.Port <= 0)
			{
				_view.ShowMessage("Please specify a valid server port.", "Invalid port.", MessageBoxType.Error, false);
				return false;
			}

#if SFTP
			if (string.IsNullOrEmpty(connection.Username))
			{
				_view.ShowMessage("Please specify user name.", "User name missing.", MessageBoxType.Error, false);
				return false;
			}

			if (string.IsNullOrEmpty(connection.Password))
			{
				_view.ShowMessage("Password missing.", "Please specify password.", MessageBoxType.Error, false);
				return false;
			}
#endif

			return true;
		}

		/// <summary>
		/// Adds the specified site to the site list.
		/// </summary>
		/// <param name="info">The connection info.</param>
		public void AddSite(IConnectionInfo info)
		{
			// TODO: Add your code here.
		}

		/// <summary>
		/// Removes the specified site.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveSite(int index)
		{
			// TODO: Add your code here.
		}

		/// <summary>
		/// Saves the site list.
		/// </summary>
		public void Save()
		{
		}

		/// <summary>
		/// Loads the site list.
		/// </summary>
		public void Load()
		{
			list = new List<IConnectionInfo> ();

			IConnectionInfo info = new ConnectionInfo ();
			info.Name = "ComponentPro SFTP";
			info.Server = "demo.componentpro.com";
			info.Port = 22;
			info.Username = "test";
			info.Password = "test";

			list.Add (info);
		}

		/// <summary>
		/// Gets the connection info list.
		/// </summary>
		/// <returns>The connection info list.</returns>
		public IConnectionInfo[] GetConnectionInfoList()
		{
			return list.ToArray ();
		}
	}
}