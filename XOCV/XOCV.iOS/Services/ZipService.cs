using System;
using ICSharpCode.SharpZipLib.Zip;
using XOCV.Services;

namespace XOCV.iOS.Services
{
    public class ZipService : IZipService
    {
        public void PackZipFile()
        {
            FastZip fastZip = new FastZip();

            bool recurse = true;
            string filter = @"\.jpeg$";
            string fileName = "Images_" + DateTime.Now.ToString("yyyy-MM-dd") + ".zip";
            string pathForZipFile = String.Empty;   // ToDo: need check it.

            fastZip.CreateZip(fileName, pathForZipFile, recurse, filter);
        }

        public void UnpackZipFile(string zipFileName)
        {
            FastZip fastZip = new FastZip();

            string fileFilter = null;
            string targerDit = null;    // ToDo: need check it.

            fastZip.ExtractZip(zipFileName, targerDit, fileFilter);
        }
    }
}