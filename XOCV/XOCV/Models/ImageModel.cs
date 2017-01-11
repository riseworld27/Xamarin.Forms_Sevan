using System;
using Newtonsoft.Json;
using PropertyChanged;

namespace XOCV.Models
{
    [ImplementPropertyChanged]
    public class ImageModel
    {
        [JsonProperty (PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "imagePosition")]
        public int ImagePosition { get; set; }

        [JsonProperty (PropertyName = "thumbnailImage")]
        public MediaFile ThumbnailImage { get; set; }

        [JsonProperty(PropertyName = "fullSizeImage")]
        public MediaFile FullSizeImage { get; set; }
    }
}