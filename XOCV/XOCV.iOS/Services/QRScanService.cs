using System.Threading.Tasks;

namespace XOCV.iOS.Services
{
    public class QRScanService : IQRScanService
    {
        public async Task<string> ScanAsync ()
        {
            string qrInfo = null;
            var scanner = new ZXing.Mobile.MobileBarcodeScanner ();
            var result = await scanner.Scan ();

            qrInfo = result?.Text;

            return qrInfo;
        }
    }
}