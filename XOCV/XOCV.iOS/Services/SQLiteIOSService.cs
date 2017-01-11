using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;
using XOCV.iOS.Service;
using XOCV.Services;

[assembly: Dependency(typeof(SQLiteIOSService))]
namespace XOCV.iOS.Service
{
    public class SQLiteIOSService : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "SevanSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            // This is where we copy in the prepopulated database
            Console.WriteLine(path);
            if (!File.Exists(path))
            {
                File.Copy(sqliteFilename, path);
            }

            var conn = new SQLite.Net.SQLiteConnection(new SQLitePlatformIOS(), path, SQLiteOpenFlags.ReadWrite);

            // Return the database connection 
            return conn;
        }
    }
}