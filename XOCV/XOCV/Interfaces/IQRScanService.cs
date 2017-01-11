using System.Threading.Tasks;

namespace XOCV
{
    public interface IQRScanService
    {
        Task<string> ScanAsync ();
    }
}