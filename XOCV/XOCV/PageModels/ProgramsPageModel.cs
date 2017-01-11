using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Models.ResponseModels;
using XOCV.ViewModels.Base;

namespace XOCV.PageModels
{
    [ImplementPropertyChanged]
    public class ProgramsPageModel : BasePageModel
    {
        #region Properties
        public ObservableCollection<ProgramModel> Programs { get; set; }
        public CompanyModel Company { get; set; }
        #endregion

        public IUserDialogs UserDialogsService { get; private set; }

        #region Commands
        public ICommand OpenProgramCommand => new Command<ProgramModel>(async (programModel) => await OpenProgramCommandExecute(programModel));
        #endregion

        #region Constructors
        public ProgramsPageModel()
        {
            
        }
        public ProgramsPageModel(IUserDialogs userDialogs)
        {
            UserDialogsService = userDialogs;
        }
        #endregion

        #region Initialize
        public override void Init(object initData)
        {
            base.Init(initData);

            if (initData is CompanyModel)
            {
                Company = (CompanyModel)initData;
                Programs = Company.ListOfPrograms;
            }
        }
        #endregion

        #region Commands Execution
        private async Task OpenProgramCommandExecute(ProgramModel programModel)
        {
            UserDialogsService.ShowLoading();
			await CoreMethods.PushPageModel<FormsPageModel>(programModel);
            UserDialogsService.HideLoading();
        }
        #endregion

        #region Additionals methods

        #endregion
    }
}
