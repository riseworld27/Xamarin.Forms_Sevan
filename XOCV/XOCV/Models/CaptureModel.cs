using System;
using Newtonsoft.Json;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using XOCV.Enums;
using XOCV.Models.ResponseModels;

namespace XOCV.Models
{
    [ImplementPropertyChanged]
    public class CaptureModel
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int CaptureModelID { get; set; }

        [JsonIgnore, ForeignKey (typeof (ComplexFormsModel))]
        public int ComplexFormsModelID { get; set; }

        [JsonProperty (PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty (PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty (PropertyName = "storeNumber")]
        public string StoreNumber { get; set; }

        [JsonProperty (PropertyName = "formStatus")]
        public FormStatus FormStatus { get; set; }

        [JsonProperty (PropertyName = "syncStatus")]
        public SyncStatus SyncStatus { get; set; }
    }
}