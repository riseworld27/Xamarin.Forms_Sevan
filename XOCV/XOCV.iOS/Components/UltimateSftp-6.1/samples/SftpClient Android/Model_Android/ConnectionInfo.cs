using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Java.Interop;
using System;

namespace ComponentProSamples
{
	/// <summary>
	/// Extends the IObjectInfo for Android.
	/// </summary>
	public partial interface IObjectInfo : IParcelable
	{
	}

	/// <summary>
	/// Extends the ConnectionInfo for Android.
	/// </summary>
	public partial class ConnectionInfo : Java.Lang.Object
	{
		// The creator creates an instance of the specified object
		private static readonly GenericParcelableCreator<ConnectionInfo> _creator
		= new GenericParcelableCreator<ConnectionInfo>((parcel) => new ConnectionInfo(parcel));

		[ExportField("CREATOR")]
		public static GenericParcelableCreator<ConnectionInfo> GetCreator()
		{
			return _creator;
		}

		// Create a new SelectListItem populated with the values in parcel
		private ConnectionInfo(Parcel parcel)
		{
			Server = parcel.ReadString();
			Port = parcel.ReadInt();
			Username = parcel.ReadString();
			Password = parcel.ReadString();
		}

		public int DescribeContents()
		{
			return 0;
		}

		// Save this instance's values to the parcel
		public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
		{
			dest.WriteString(Server);
			dest.WriteInt(Port);
			dest.WriteString(Username);
			dest.WriteString(Password);
		}

		public IParcelable[] ToList()
		{
			List<IParcelable> list = new List<IParcelable> ();
			list.Add (this);
			return list.ToArray();
		}
	}
}
