using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;
using XOCV.Models.ResponseModels;
using XOCV.ViewModels.Base;

namespace XOCV.PageModels
{
    public class FormsPageModel : BasePageModel
    {
        public ProgramModel Program { get; set; }
        public ObservableCollection<ComplexFormsModel> Forms { get; set; }

        public IUserDialogs UserDialogsService { get; private set; }

        public ICommand OpenFormDetailsCommand => new Command<ComplexFormsModel>(async (form) => await OpenFormDetailsCommandExecute(form));

        public FormsPageModel()
        {
            
        }
        public FormsPageModel(IUserDialogs userDialogs)
        {
            UserDialogsService = userDialogs;
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            if (initData is ProgramModel)
            {
                Program = (ProgramModel) initData;
                Forms = Program.SetOfForms;
            }
        }

        private async Task OpenFormDetailsCommandExecute(ComplexFormsModel form)
        {
            UserDialogsService.ShowLoading();
			long formId = form.FormId;
			object[] args = { form, formId };
            await CoreMethods.PushPageModel<FormDetailsPageModel>(args);
            UserDialogsService.HideLoading();
        }
    }
}