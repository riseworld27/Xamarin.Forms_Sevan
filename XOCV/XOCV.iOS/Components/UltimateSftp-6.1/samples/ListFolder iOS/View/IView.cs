using System;

namespace ComponentProSamples
{
	/// <summary>
	/// Message box type.
	/// </summary>
	public enum MessageBoxType
	{
		/// <summary>
		/// Warning state.
		/// </summary>
		Warning,
		/// <summary>
		/// Error state.
		/// </summary>
		Error,
		/// <summary>
		/// Question state.
		/// </summary>
		Question,
		/// <summary>
		/// Information state.
		/// </summary>
		Information
	}

	/// <summary>
	/// Defines the general view object.
	/// </summary>
	public interface IView
	{
		/// <summary>
		/// Sets the status.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="args">Arguments.</param>
		void SetStatus(string message, params object[] args);

		/// <summary>
		/// Sets the busy state.
		/// </summary>
		/// <param name="busy">If set to <c>true</c> busy.</param>
		void SetBusy(bool busy);

		/// <summary>
		/// Shows a message to the user.
		/// </summary>
		/// <param name="message">Message to show.</param>
		/// <param name="title">Title of the message box.</param>
		/// <param name="type">Message box type.</param>
		/// <param name="terminate"><c>true</c> to terminate the view; otherwise <c>false</c>.</param>
		void ShowMessage(string message, string title, MessageBoxType type, bool terminate);

		/// <summary>
		/// Shows the alert.
		/// </summary>
		/// <returns>0: OK, [last]: Cancel.</returns>
		/// <param name="message">Message.</param>
		/// <param name="title">Title.</param>
		/// <param name="options">[0]: "OK" button text, [last]: "Cancel" button text.</param>
        int ShowAlert(string message, string title, params string[] options);
	}
}

