using System.Collections.ObjectModel;
using Newtonsoft.Json;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XOCV.Models.ResponseModels
{
    [ImplementPropertyChanged]
    public class Item
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int ID { get; set; }

        [JsonIgnore, ForeignKey (typeof (ComplexItem))]
        public int ComplexItemID { get; set; }

        [JsonProperty (PropertyName = "itemId")]
        public int ItemId { get; set; }

        [JsonProperty (PropertyName = "itemName")]
        public string Name { get; set; }

        [ForeignKey (typeof (PropertyModel)), JsonIgnore]
        public int PropertyModelId { get; set; }

        [OneToOne, JsonProperty (PropertyName = "properties")]
        public PropertyModel Properties { get; set; }

        [JsonProperty (PropertyName = "itemsOfDictionaryIntToString"), TextBlob ("RadioButtonItemsBlobbed")]
        public ObservableCollection<string> RadioButtonItemsSource { get; set; }

        [JsonIgnore]
        public string ImagesItemsBlobbed { get; set; }

        [JsonIgnore]
        public string RadioButtonItemsBlobbed { get; set; }

        [JsonProperty (PropertyName = "isMultipleSelection")]
        public bool IsMultipleSelection { get; set; }

        [JsonProperty (PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty (PropertyName = "values")]
        public ObservableCollection<string> Values { get; set; }

        [JsonProperty (PropertyName = "images"), TextBlob ("ImagesItemsBlobbed")]
        public ObservableCollection<string> Images { get; set; }

        [JsonIgnore]
        public string ListOfImageBlobbed { get; set; }

        public Item ()
        {
            IsMultipleSelection = false;
            Value = string.Empty;
            Images = new ObservableCollection<string> ();
            Values = new ObservableCollection<string> ();
            RadioButtonItemsSource = new ObservableCollection<string> ();
            Properties = new PropertyModel ();
        }
    }
}