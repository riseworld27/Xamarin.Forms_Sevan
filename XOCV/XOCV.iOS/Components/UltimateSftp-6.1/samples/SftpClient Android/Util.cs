using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ComponentPro.IO;
using ComponentPro.Net;

namespace ComponentProSamples
{
	public static class Util
	{
		public const string ConnectionKey = "connection";

		/// <summary>
		/// Determines if a file is a picture.
		/// </summary>
		/// <param name="name">File name.</param>
		/// <returns>True if picture.</returns>
		public static bool IsPicture(string name)
		{
			string extension = Path.GetExtension(name).ToLowerInvariant();

			return extension == ".tiff" || extension == ".tif" || extension == ".jpg"
				|| extension == ".jpeg" || extension == ".gif" || extension == ".png"
				|| extension == ".bmp" || extension == ".ico";
		}

		/// <summary>
		/// Determine if a file is a music file.
		/// </summary>
		/// <param name="name">File name.</param>
		/// <returns>True if music.</returns>
		public static bool IsMusic(string name)
		{
			string extension = Path.GetExtension(name).ToLowerInvariant();

			return extension == ".mp3";
		}

		/// <summary>
		/// Shows a message to the user.
		/// </summary>
		public static void ShowMessage(string title, string message, Activity context, EventHandler dismissHandler)
		{
			if (context.IsFinishing)
				return;

			context.RunOnUiThread(() =>
				{
					AlertDialog dialog = new AlertDialog.Builder(context)
						.SetTitle(title)
						.SetMessage(message)
						.Create();

					if (dismissHandler != null)
						dialog.DismissEvent += dismissHandler;
					dialog.Show();
				}
			);
		}
	}
}