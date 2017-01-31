using System;

namespace SftpClientSample
{
	public interface ISettings
	{
		int GetInt (string varName, int defaultValue);

		string GetString (string varName, string defaultValue);

		void Set (string varName, string value);

		void Set (string varName, int value);
	}
}

