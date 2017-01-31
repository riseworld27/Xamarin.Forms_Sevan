using XOCV.PageModels.Base;
using PropertyChanged;
using XOCV.Helpers;
using System.Windows.Input;
using Xamarin.Forms;

namespace XOCV.PageModels
{
	[ImplementPropertyChanged]
	public class SettingsPageModel : BasePageModel
	{
		public bool AllowAdvancedMode { get; set; }

		public ICommand ApplyCommand => new Command(() => ApplyCommandExecute());
		public ICommand CloseCommand => new Command(() => CloseCommandExecute());
		public SettingsPageModel()
		{
		}
		public override void Init(object initData)
		{
			base.Init(initData);
			AllowAdvancedMode = Settings.AllowAdvancedMode;
		}

		void ApplyCommandExecute()
		{
			Settings.AllowAdvancedMode = AllowAdvancedMode;
			CoreMethods.PopPageModel();
		}

		void CloseCommandExecute()
		{
			CoreMethods.PopPageModel();
		}
	}
}
