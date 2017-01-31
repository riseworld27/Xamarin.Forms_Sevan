using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System;

namespace ComponentProSamples
{
	/// <summary>
	/// Generic parcelable creator.
	/// </summary>
	public sealed class GenericParcelableCreator<T> : Java.Lang.Object, IParcelableCreator
		where T : Java.Lang.Object, new()
	{
		private readonly Func<Parcel, T> _createFunc;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParcelableCreator{T}"/> class.
		/// </summary>
		/// <param name='createFromParcelFunc'>
		/// Func that creates an instance of T, populated with the values from the parcel parameter
		/// </param>
		public GenericParcelableCreator(Func<Parcel, T> createFromParcelFunc)
		{
			_createFunc = createFromParcelFunc;
		}

		#region IParcelableCreator Implementation

		/// <param name="source">The Parcel to read the object's data from.</param>
		/// <summary>
		/// Creates from parcel.
		/// </summary>
		/// <returns>The from parcel.</returns>
		public Java.Lang.Object CreateFromParcel(Parcel source)
		{
			return _createFunc(source);
		}

		/// <param name="size">Size of the array.</param>
		/// <summary>
		/// Create a new array of the Parcelable class.
		/// </summary>
		/// <returns>To be added.</returns>
		public Java.Lang.Object[] NewArray(int size)
		{
			return new T[size];
		}

		#endregion
	}
}

