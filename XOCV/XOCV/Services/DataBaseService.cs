using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;
using XOCV.Models;
using XOCV.Models.ResponseModels;

namespace XOCV.Services
{
    public class DataBaseService
    {
        private static object locker = new object ();
        private SQLiteConnection database;

        public int CountComplexForms { get; set; } = 0;

        public DataBaseService ()
        {
            database = DependencyService.Get<ISQLite> ().GetConnection ();
            database.CreateTable<DBModel> ();
        }

        #region GetMetods
        public DBModel GetFirstComplexFormsModel ()
        {
            lock (locker)
            {
                var firstComplexFormModel = database.GetWithChildren<DBModel> (1, true);
                return firstComplexFormModel;
            }
        }

        public DBModel GetContentById (int id)
        {
            lock (locker)
            {
                var item = database.GetWithChildren<DBModel> (id, true);
                return item;
            }
        }

        public List<DBModel> GetContent ()
        {
            lock (locker)
            {
                var result = database.GetAllWithChildren<DBModel> ();
                return result;
            }
        }

        public List<ComplexFormsModel> GetAllComplexFormContent (bool getOnlySelected = false)
        {
            lock (locker)
            {
                var allContentFromLocalDb = database.GetAllWithChildren<DBModel>();

                List<ComplexFormsModel> complexFormsModels = new List<ComplexFormsModel>();

                if (getOnlySelected == false)
                {
                    complexFormsModels.AddRange(allContentFromLocalDb.Select(item => JsonConvert.DeserializeObject<ComplexFormsModel>(item.Content)));
                }
                else
                {
                    complexFormsModels.AddRange(from item in allContentFromLocalDb where item.IsSelected == true select JsonConvert.DeserializeObject<ComplexFormsModel>(item.Content));
                }

                return complexFormsModels;
            }
        }
        #endregion

        #region SaveMetods
        public void SaveContent (DBModel item)
        {
            lock (locker)
            {
                database.InsertOrReplaceWithChildren (item);
            }
        }
        #endregion

        #region DeleteMetods
        public void DeleteItem (DBModel item)
        {
            lock (locker)
            {
                database.Delete (item, recursive: true);
            }
        }

		//public void ClearAllDataBase()
		//{
		//    lock (locker)
		//    {
		//        database.DeleteAll<DBModel>();
		//    }
		//}
		#endregion
    }
}