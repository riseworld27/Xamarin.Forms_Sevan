using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XOCV.Models.ResponseModels
{
    public class StoreNumberModel
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int StoreNumberModelID { get; set; }

        [JsonIgnore, ForeignKey(typeof(ComplexFormsModel))]
        public int ComplexFormsModelID { get; set; }

        [JsonProperty(PropertyName = "storeNumber")]
        public int StoreNumber { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }

        [JsonProperty(PropertyName = "contractor")]
        public string Contractor { get; set; }
    }
}