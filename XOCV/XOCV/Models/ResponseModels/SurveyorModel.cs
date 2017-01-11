using Newtonsoft.Json;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XOCV.Models.ResponseModels
{
    [ImplementPropertyChanged]
    public class SurveyorModel
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int SurveyorModelID { get; set; }

        [JsonIgnore, ForeignKey(typeof(ComplexFormsModel))]
        public int ComplexFormsModelID { get; set; }

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}