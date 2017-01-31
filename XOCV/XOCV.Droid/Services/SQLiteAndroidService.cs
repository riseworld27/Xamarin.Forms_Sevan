using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using XOCV.Droid.Services;
using XOCV.Services;

[assembly:Xamarin.Forms.Dependency(typeof(SQLiteAndroidService))]
namespace XOCV.Droid.Services
{
    public class SQLiteAndroidService : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "SevanSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            
            var conn = new SQLite.Net.SQLiteConnection(new SQLitePlatformAndroid(), path);

            // Return the database connection 
            return conn;
        }
    }
}