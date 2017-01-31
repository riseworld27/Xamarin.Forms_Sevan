using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XOCV.Models.ResponseModels
{
    public class ItemImageModel
    {
        public Guid Id { get; set; }

        [PrimaryKey, AutoIncrement]
        public int ItemImageModelID { get; set; }

        [ForeignKey (typeof (ComplexItem))]
        public int ComplexItemID { get; set; }

        public string Base64String { get; set; }
    }
}