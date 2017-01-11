using System;
using ComponentPro.IO;

namespace ComponentProSamples
{
	/// <summary>
	/// Defines the client browser view methods.
	/// </summary>
	public interface IClientBrowserView : IView
	{
		/// <summary>
		/// Sets the path when user change the current directory.
		/// </summary>
		/// <param name="path">Path.</param>
		void SetPathText(string path);

		/// <summary>
		/// Lists the directory.
		/// </summary>
		/// <param name="list">List.</param>
		void ListDirectory(FileInfoCollection list);

		/// <summary>
		/// Shows the context menu.
		/// </summary>
		/// <param name="itemView">Item view.</param>
		void ShowContextMenu(AbstractFileInfo item, object itemView);
	}
}

