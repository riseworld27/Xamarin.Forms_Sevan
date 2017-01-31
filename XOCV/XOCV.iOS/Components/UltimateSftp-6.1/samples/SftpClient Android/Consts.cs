using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ComponentPro.Net;

namespace SftpClientSample
{
	/// <summary>
	/// Sample constants.
	/// </summary>
	internal static class Util
	{
		public const int DefaultRequestId = 0;

		public const long MaxPreviewLength = 10 * 1024;

		public const string UpText = "..";

		public const string SiteIdKey = "SiteId";
		public const string DescriptionKey = "Description";
		public const string ServerNameKey = "ServerName";
		public const string PortKey = "Port";
		public const string UserNameKey = "UserName";
		public const string PasswordKey = "Password";

		public const int DefaultPort = Sftp.DefaultPort;

		public const string CurrentDirectoryKey = "CurrentDirectory";
		
	}
}