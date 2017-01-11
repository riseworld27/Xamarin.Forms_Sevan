using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace XOCV.Models.ResponseModels
{
    public class CompanyModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "listOfPrograms")]
        public ObservableCollection<ProgramModel> ListOfPrograms { get; set; }

        public CompanyModel()
        {
            ListOfPrograms = new ObservableCollection<ProgramModel>();
        }
    }
}