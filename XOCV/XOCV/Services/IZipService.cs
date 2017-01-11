using System.Threading.Tasks;

namespace XOCV.Services
{
    public interface IZipService
    {
        void PackZipFile();
        void UnpackZipFile(string zipFileName);
    }
}
