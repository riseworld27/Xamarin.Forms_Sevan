using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace XOCV.Models.ResponseModels
{
    public class ProgramModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

		[JsonProperty(PropertyName = "setOfForms")]
        public ObservableCollection<ComplexFormsModel> SetOfForms { get; set; }

        public ProgramModel()
        {
            SetOfForms = new ObservableCollection<ComplexFormsModel>();
        }
    }
}