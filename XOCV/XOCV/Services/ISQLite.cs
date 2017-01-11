using SQLite.Net;

namespace XOCV.Services
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}