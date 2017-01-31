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
			database.CreateTable<LocalContentModel>();
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

        public List<DBModel> GetContent (bool getOnlySelected = false)
        {
            lock (locker)
            {
                var dBModels = database.GetAllWithChildren<DBModel>();
                return getOnlySelected ? dBModels.Where(m => m.IsSelected).ToList() : dBModels;
            }
        }

		public LocalContentModel GetLocalContent()
		{
			lock (locker)
			{
				var localContentModel = database.GetWithChildren<LocalContentModel>(1, true);
				return localContentModel;
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

		public void SaveLocalContent(LocalContentModel localContent)
		{
			lock (locker)
			{
				localContent.ID = 1;
				database.InsertOrReplaceWithChildren(localContent);
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
		#endregion
    }
}