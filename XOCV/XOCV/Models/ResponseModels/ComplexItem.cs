using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XOCV.Models.ResponseModels
{
    [ImplementPropertyChanged]
    public class ComplexItem
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int ComplexItemID { get; set; }

        [JsonIgnore, ForeignKey (typeof (FormModel))]
        public int FormModelID { get; set; }

        [JsonProperty (PropertyName = "items"), OneToMany (CascadeOperations = CascadeOperation.All)]
        public List<Item> Items { get; set; }

        [JsonProperty (PropertyName = "isRepeatableSection")]
        public bool IsRepeatableSection { get; set; }

		[JsonProperty(PropertyName = "isRepeatedSection")]
		public bool IsRepeatedSection { get; set; }

        [JsonProperty(PropertyName = "isGridStyle")]
        public bool IsGridStyle { get; set; }

        public ObservableCollection<ObservableCollection<string>> ItemImageModels { get; set; }

        public ComplexItem ()
        {
            Items = new List<Item> ();
            ItemImageModels = new ObservableCollection<ObservableCollection<string>> ();
        }

		public ComplexItem GetRepeatedSection()
		{
			var json = JsonConvert.SerializeObject(this);
			ComplexItem clone = JsonConvert.DeserializeObject<ComplexItem>(json);
			clone.IsRepeatableSection = false;
			clone.IsRepeatedSection = true;
			foreach (var item in clone.Items)
			{
				switch (item.Name)
				{
					case "entryKey":
						{
							item.Value = string.Empty;
							break;
						}
					case "radioGroupKey":
						{
							item.Value = string.Empty;
							item.Values.Clear();
							break;
						}
					case "imageKey":
						{
							item.Images.Clear();
							break;
						}
				}
			}
			return clone;
		}
    }
}