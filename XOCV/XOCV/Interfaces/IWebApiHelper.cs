using System.Threading.Tasks;
using XOCV.Models;
using XOCV.Models.ResponseModels;

namespace XOCV.Interfaces
{
    public interface IWebApiHelper
    {
        Task<LoginResponseModel> Authorization(LoginModel login);
        Task<ContentModel> GetAllContent(string token);
        Task<bool> PostAllContent(ProgramModel model);
    }
}