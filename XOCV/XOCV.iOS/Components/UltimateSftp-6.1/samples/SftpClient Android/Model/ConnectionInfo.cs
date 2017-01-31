using System;

namespace ComponentProSamples
{
	/// <summary>
	/// Represents the connection info.
	/// </summary>
	public partial class ConnectionInfo : IConnectionInfo, IObjectInfo
	{
		/// <summary>
		/// Gets or sets the site name.
		/// </summary>
		/// <value>The site name.</value>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the server address.
		/// </summary>
		/// <value>The server name.</value>
		public string Server 
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the port.
		/// </summary>
		/// <value>The port.</value>
		public int Port {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		public string Username
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentProSamples.ConnectionInfo"/> class.
		/// </summary>
		public ConnectionInfo()
		{
		}
	}
}

