using FreshMvvm;

namespace XOCV.ViewModels.Base
{
    public class BasePageModel : FreshBasePageModel
    {
        public bool IsVisible { get; set; }
        public bool IsBusy { get; set; }
    }
}