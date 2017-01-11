using System;
using System.IO;
using System.Text.RegularExpressions;

using UIKit;
using Foundation;
using System.Threading.Tasks;
using MonoTouch.Dialog;

namespace ComponentProSamples
{
	/// <summary>
	/// Utility class that contains some helpful methods used by the example. 
	/// </summary>
	public static class Util
	{
		public static void SaveToPhotos(NSData data)
		{
			// create an image from the downloaded data and save it to Photos
			using (UIImage image = UIImage.LoadFromData(data))
			{
				if (image == null)
				{
					Util.ShowMessage("Could not load the image.", "The image format is corrupt.");
					return;
				}

				image.SaveToPhotosAlbum((img, error) =>
				                        {
					if (error != null)
					{
						Util.ShowMessage("There was en error saving to Photos.", error.ToString());
					}
					else
					{
						Util.ShowMessage("File saved to Photos.", "");
					}
				});
			}
		}

		/// <summary>
		/// Shows a dialog to the user asynchronously.
		/// </summary>
		/// <returns>The dialog to show.</returns>
		/// <param name="dialog">The user selected button index.</param>
		public static Task<int> ShowDialogAsync(UIAlertView dialog)
		{
			var tsc = new TaskCompletionSource<int>();

			dialog.Show ();

			dialog.Clicked += (sender, e) =>  
			{
				tsc.SetResult((int)e.ButtonIndex);
			};

			return tsc.Task;
		}

		/// <summary>
		/// Shows a dialog to the user.
		/// </summary>
		/// <returns>The dialog to show.</returns>
		/// <param name="dialog">The user selected button index.</param>
		public static int ShowDialog(UIAlertView dialog)
		{
			int? result = null;
			dialog.Show();
			dialog.Clicked += (sender,e) => { result = (int)e.ButtonIndex; };

			while (result == null)
			{
				NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow(0.2));
			}
			return result.Value;
		}

		/// <summary>
		/// Shows an exception to the user.
		/// </summary>
		private static void ShowException(string title, Exception error)
		{
			while (error is AggregateException)
				error = error.InnerException;

			ShowMessage(title, error.ToString());
		}

		/// <summary>
		/// Shows an exception to the user.
		/// Exception message is used as caption.
		/// </summary>
		public static void ShowException(Exception ex)
		{
			ShowException(ex.Message, ex);
		}

		/// <summary>
		/// Shows a message to the user.
		/// </summary>
		public static void ShowMessage(string title, string message)
		{
			new UIAlertView(title, message, null, "OK", null).Show();
		}

		public static void DisableAutoCorrectionAndAutoCapitalization(EntryElement element)
		{
			element.AutocapitalizationType = UITextAutocapitalizationType.None;
			element.AutocorrectionType = UITextAutocorrectionType.No;
		}

		/// <summary>
		/// Determines whether app is running on iPhone sized device or not.
		/// </summary>
		public static bool IsIphone
		{
			get
			{
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
			}
		}

		/// <summary>
		/// Determines whether app is running on IOS7 or later.
		/// </summary>
		public static bool IsIos7OrLater
		{
			get 
			{ 
				return UIDevice.CurrentDevice.CheckSystemVersion(7, 0);
			}
		}

		/// <summary>
		/// Returns a formatted file size in bytes, kbytes, or mbytes.
		/// </summary>
		/// <param name="size">The input file size.</param>
		/// <returns>The formatted file size.</returns>
		public static string FormatSize(long size)
		{
			if (size < 1024)
				return size + " B";
			if (size < 1024 * 1024)
				return string.Format("{0:#.#} KB", size / 1024.0f);
			return size < 1024 * 1024 * 1024 ? string.Format("{0:#.#} MB", size / 1024.0f / 1024.0f) : string.Format("{0:#.#} GB", size / 1024.0f / 1024.0f / 1024.0f);
		}
	}
}
