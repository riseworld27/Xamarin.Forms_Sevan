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
using ComponentPro.IO;
using ComponentPro.Net;

namespace ComponentProSamples
{
	/// <summary>
	/// This class represents the content of a remote folder.
	/// It provides data for the <see cref="BrowserActivity"/> activity <see cref="ListView"/>.
	/// </summary>
	public class FileInfoListAdapter : ArrayAdapter<AbstractFileInfo>
	{
		// the site activity that contains the items list
		private BrowserActivity _site;
		ClientLogic _logic;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="items">Items serving as the source for the adapter.</param>		
		/// <param name="context">The browser activity that contains the items list.</param>
		public FileInfoListAdapter(BrowserActivity context, ClientLogic logic, FileInfoCollection items)
			: base(context, Resource.Layout.RemoteItem, items)
		{
			_site = context;
			_logic = logic;
		}

		/// <summary>
		/// Creates a view for each remote file system item in this adapter.
		/// </summary>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			AbstractFileInfo item = GetItem(position);

			// get the views
			View view = _site.LayoutInflater.Inflate(Resource.Layout.RemoteItem, parent, false);
			var image = view.FindViewById<ImageView>(Resource.Id.imgItemIcon);
			var name = view.FindViewById<TextView>(Resource.Id.txtItemName);

			// store the file name within the view to make it accessible from the context menu item click handler
			// (BrowserActivity.OnContextItemSelected method)
			view.SetTag(Resource.Id.ItemName, new Java.Lang.String(item.Name));
				
			name.Text = item.Name;

			if (item.IsDirectory)
			{
				if (item.Name == "..")
					image.SetImageResource(Resource.Drawable.up);
				else
					image.SetImageResource(Resource.Drawable.folder);
			}
			else if (item.IsFile)
			{
				image.SetImageResource(Resource.Drawable.file);
			}
			else if (item.IsSymlink)
			{
				image.SetImageResource(Resource.Drawable.link);
			}

			view.Click += (s, e) => ItemClick(item, view);

			return view;
		}

		/// <summary>
		/// Handles File Item click event.
		/// </summary>
		/// <param name="item">Remote item representing the file.</param>
		/// <param name="itemView">View for the item.</param>	
		private void ItemClick(AbstractFileInfo item, View itemView)
		{
			_logic.OnItemClicked (item, itemView);
		}
	}
}