using System.Threading.Tasks;
using XOCV.iOS.Services;
using XOCV.Services;

[assembly: Xamarin.Forms.Dependency(typeof(EmailService))]
namespace XOCV.iOS.Services
{
    public class EmailService : IEmailService
    {
        public Task<bool> SendEmail()
        {
            throw new System.NotImplementedException(); // ToDo: implement in future
        }
    }
}