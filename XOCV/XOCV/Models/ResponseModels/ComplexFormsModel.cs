using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PropertyChanged;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace XOCV.Models.ResponseModels
{
    [ImplementPropertyChanged]
    public class ComplexFormsModel
    {
        [PrimaryKey, AutoIncrement, JsonIgnore]
        public int ComplexFormsModelID { get; set; }

        [JsonProperty (PropertyName = "formId")]
        public long FormId { get; set; }

        [JsonProperty (PropertyName = "title")]
        public string FormsTitle { get; set; }

        [JsonProperty (PropertyName = "storeNumber")]
        public int StoreNum { get; set; }

        [JsonProperty (PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty (PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty (PropertyName = "captureGuid")]
        public Guid CaptureGuid { get; set; }

        [JsonProperty (PropertyName = "instructions")]
        public string Instructions { get; set; }

        [JsonProperty (PropertyName = "formCaptures"), OneToMany (CascadeOperations = CascadeOperation.All)]
        public List<CaptureModel> Captures { get; set; }

        [JsonProperty (PropertyName = "surveyors"), OneToMany (CascadeOperations = CascadeOperation.All)]
        public List<SurveyorModel> Surveyors { get; set; }

        [JsonIgnore]
        public string Surveyor { get; set; }

        [JsonProperty (PropertyName = "dateOfSurvey")]
        public string DateOfSurvey { get; set; }

        [JsonProperty (PropertyName = "storeNumbers"), OneToMany (CascadeOperations = CascadeOperation.All)]
        public List<StoreNumberModel> StoreNumbers { get; set; }

        [JsonIgnore]
        public string StoreNumber { get; set; }

        [JsonIgnore]
        public string BrandingPackage { get; set; }

        [JsonProperty (PropertyName = "mainEntranceDoorPhoto"), Ignore]
        public ImageModel MainEntranceDoorPhoto { get; set; }

        [JsonProperty (PropertyName = "forms"), OneToMany (CascadeOperations = CascadeOperation.All)]
        public List<FormModel> Forms { get; set; }

        public ComplexFormsModel ()
        {
            Captures = new List<CaptureModel> ();
            Surveyors = new List<SurveyorModel> ();
            StoreNumbers = new List<StoreNumberModel> ();
            Forms = new List<FormModel> ();
        }
    }
}