using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using XOCV.Models.ResponseModels;

namespace XOCV.Models
{
    public class ContentModel
    {
        [JsonProperty(PropertyName = "listOfCompanies")]
        public ObservableCollection<CompanyModel> ListOfCompanies { get; set; }

        public ContentModel()
        {
            ListOfCompanies = new ObservableCollection<CompanyModel>();
        }
    }
}