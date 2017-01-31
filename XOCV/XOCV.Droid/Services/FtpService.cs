using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ComponentPro.Net;
using Newtonsoft.Json;
using XOCV.Droid.Services;
using XOCV.Models.ResponseModels;
using XOCV.Services;

[assembly: Xamarin.Forms.Dependency (typeof (FtpService))]
namespace XOCV.Droid.Services
{
    public class FtpService : IFtpService
    {
        private Sftp Client { get; }
        private string DocumentsDirectory { get; }
        public string TempBackUpFolderName { get; set; }

        // Connection
        private readonly string ServerName = "50.112.202.60";
        private readonly int ServerPort = 22;

        // Credentials
        private readonly string UserName = "sevan-img.pilgrimconsulting.com";
        private readonly string Password = "8A$sevan";

        public FtpService()
        {
            Client = new Sftp();
            DocumentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public async Task<bool> SendJsonFile(ComplexFormsModel model)
        {
            if (model == null) return false;

            bool success;

            // Connect to the SFTP server.
            Client.Connect(ServerName, ServerPort);

            // Authenticate.
            Client.Authenticate(UserName, Password);

            try
            {
                var storeNumber = model.StoreNum + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".json";
                var dirpath = string.Format("/home/sevan-img.pilgrimconsulting.com/captures/PSN/{0}", model.StoreNum);

                if (!Client.DirectoryExists(dirpath))
                {
                    Client.CreateDirectory(dirpath);
                }

                string json = JsonConvert.SerializeObject(model);
                string jsonFilename = Path.Combine(DocumentsDirectory, storeNumber);

                File.WriteAllText(jsonFilename, json);

                var fileStream = new FileStream(jsonFilename, FileMode.Open, FileAccess.Read);

                var r = Client.UploadFile(fileStream, dirpath + "/" + storeNumber);
                fileStream.Flush();
                fileStream.Close();

                success = r != 0;
            }
            catch (Exception ex)
            {
                var exMes = ex.Message;
                Debug.WriteLine(exMes);
                success = false;
            }

            Client.Disconnect();

            return success;
        }

        public async Task<bool> BackUpAllLocalDataBase(string localDbContent)
        {
            if (localDbContent == null) return false;

            bool success;

            // Connect to the SFTP server.
            Client.Connect(ServerName, ServerPort);

            // Authenticate.
            Client.Authenticate(UserName, Password);

            try
            {
                var fileName = "BackupLocalDataBase_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");

                TempBackUpFolderName = fileName;

                var dirpath = string.Format("/home/sevan-img.pilgrimconsulting.com/backups/{0}", fileName);

                if (!Client.DirectoryExists(dirpath))
                {
                    Client.CreateDirectory(dirpath);
                }

                string jsonFilename = Path.Combine(DocumentsDirectory, fileName);

                File.WriteAllText(jsonFilename, localDbContent);

                var fileStream = new FileStream(jsonFilename, FileMode.Open, FileAccess.Read);

                var result = Client.UploadFile(fileStream, dirpath + "/" + fileName + ".json");
                fileStream.Flush();
                fileStream.Close();

                success = result != 0;
            }
            catch (Exception ex)
            {
                var exMes = ex.Message;
                success = false;
            }

            Client.Disconnect();

            return success;
        }

        public async Task<bool> BackUpImages(List<string> imageNames)
        {
            if (imageNames.Count == 0)
                return false;
            bool success = true;

            // Connect to the SFTP server.
            Client.Connect("50.112.202.60", 22);

            // Authenticate.
            Client.Authenticate("sevan-img.pilgrimconsulting.com", "8A$sevan");

            foreach (string name in imageNames)
            {
                try
                {
                    string storNum = name.Split('_')[0];
                    var dirpath = string.Format("/home/sevan-img.pilgrimconsulting.com/backups/{0}", TempBackUpFolderName);
                    if (!Client.DirectoryExists(dirpath))
                    {
                        Client.CreateDirectory(dirpath);
                    }

                    string jpgFilename = Path.Combine(DocumentsDirectory, name);
                    var fileStream = new FileStream(jpgFilename, FileMode.Open, FileAccess.Read);

                    var r = Client.UploadFile(fileStream, dirpath + "/" + name);
                    fileStream.Flush();
                    fileStream.Close();

                    if (r == 0)
                    {
                        success = false;
                    }
                }
                catch (Exception ex)
                {
                    var exMes = ex.Message;
                }
            }
            Client.Disconnect();
            return success;
        }
    }
}
