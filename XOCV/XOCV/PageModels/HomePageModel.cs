using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using PropertyChanged;
using Xamarin.Forms;
using XOCV.Models.ResponseModels;
using XOCV.PageModels.Base;

namespace XOCV.PageModels
{
    [ImplementPropertyChanged]
    public class HomePageModel : BasePageModel
    {
        public ObservableCollection<CompanyModel> Companies { get; set; }

        public IUserDialogs UserDialogsService { get; private set; }

        public ICommand OpenCompanyCommand => new Command<CompanyModel>(async (companyModel) => await OpenCompanyCommandExecute(companyModel));

        public HomePageModel()
        {
            
        }
        public HomePageModel(IUserDialogs userDialogs)
        {
            UserDialogsService = userDialogs;
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            Companies = (ObservableCollection<CompanyModel>) initData;
        }

        private async Task OpenCompanyCommandExecute(CompanyModel companyModel)
        {
            UserDialogsService.ShowLoading();
            await CoreMethods.PushPageModel<ProgramsPageModel>(companyModel);
            UserDialogsService.HideLoading();
        }
    }
}