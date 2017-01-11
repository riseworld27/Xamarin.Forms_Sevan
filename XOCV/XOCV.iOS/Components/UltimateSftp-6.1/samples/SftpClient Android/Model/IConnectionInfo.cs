using System;

namespace ComponentProSamples
{
	/// <summary>
	/// Defines the connection info.
	/// </summary>
	public partial interface IConnectionInfo : IObjectInfo
	{
		/// <summary>
		/// Gets or sets the site name.
		/// </summary>
		/// <value>The site name.</value>
		string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the server address.
		/// </summary>
		/// <value>The server name.</value>
		string Server 
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the port.
		/// </summary>
		/// <value>The port.</value>
		int Port {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		string Username
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		string Password
		{
			get;
			set;
		}
	}
}

