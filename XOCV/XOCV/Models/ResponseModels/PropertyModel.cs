using Newtonsoft.Json;
using SQLite.Net.Attributes;
using XOCV.Enums;

namespace XOCV.Models.ResponseModels
{
    public class PropertyModel
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int PropertyModelId { get; set; }
        
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

		[JsonProperty(PropertyName = "imageUrl")]
		public string ImageUrl { get; set; }

		[JsonProperty(PropertyName = "localImagePath")]
		public string LocalImagePath { get; set; }

		[JsonProperty(PropertyName = "keyboardType")]
		public KeyboardType KeyboardType { get; set; }
    }
}