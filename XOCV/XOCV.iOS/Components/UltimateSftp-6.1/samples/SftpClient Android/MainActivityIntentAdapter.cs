using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;

namespace SftpClientSamples
{
	/// <summary>
	/// This class is data provider for the main activity lists (either site list or 'About' list).
	/// </summary>
	public class MainActivityIntentAdapter : ArrayAdapter<Intent>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="context">Activity to which this instance belongs to.</param>
		public MainActivityIntentAdapter(Activity context)
			: base(context, Resource.Layout.ListItem)
		{
		}

		/// <summary>
		/// Creates a view for each item in this list adapter (e.g., for remote site or about item).
		/// </summary>		
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			Intent item = GetItem(position);

			var containingActivity = ((Activity)Context);
			View view = containingActivity.LayoutInflater.Inflate(Resource.Layout.ListItem, parent, false);

			// find text view element 
			TextView lblListItemName = view.FindViewById<TextView>(Resource.Id.lblName);

			// set item description
			lblListItemName.Text = item.GetStringExtra(Consts.DescriptionKey);

			// make it clickable
			view.Click += (s, e) => containingActivity.StartActivityForResult(item, Consts.DefaultRequestId);
			view.LongClickable = true;

			return view;
		}
	}
}