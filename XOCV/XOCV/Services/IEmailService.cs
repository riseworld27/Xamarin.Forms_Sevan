using System.Threading.Tasks;

namespace XOCV.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(); // ToDo: need send .txt file and archive. Need check it with Ira.
    }
}