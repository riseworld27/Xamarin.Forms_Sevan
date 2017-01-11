using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ComponentProSamples
{
	/*public class AndroidSettings : ISettings
	{
		public Bundle InstanceState;
		public Intent Intent;

		public AndroidSettings (Intent intent, Bundle savedInstanceState)
		{
			InstanceState = savedInstanceState;
			Intent = intent;
		}

		public int GetInt(string varName, int defaultValue)
		{
			int value;
			if (InstanceState != null) {
				value = InstanceState.GetInt (varName);
				if (value == 0)
					return defaultValue;
			}
			else if (Intent != null)
				return Intent.GetIntExtra (varName, defaultValue);

			return defaultValue;
		}

		public string GetString(string varName, string defaultValue)
		{
			string value;
			if (InstanceState != null) {
				value = InstanceState.GetString (varName);
				if (value == null)
					return defaultValue;
			} else if (Intent != null) {
				value = Intent.GetStringExtra (varName);
				if (value == null)
					return defaultValue;
			}

			return defaultValue;
		}

		public void Set(string varName, string value)
		{
			if (InstanceState != null)
				InstanceState.PutString (varName, value);
			else if (Intent != null)
				Intent.PutExtra (varName, value);
		}

		public void Set(string varName, int value)
		{
			if (InstanceState != null)
				InstanceState.PutInt (varName, value);
			else if (Intent != null)
				Intent.PutExtra (varName, value);
		}
	}*/
}

