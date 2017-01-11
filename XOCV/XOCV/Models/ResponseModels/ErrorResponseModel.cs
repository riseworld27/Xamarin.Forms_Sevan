using Newtonsoft.Json;

namespace XOCV.Models.ResponseModels
{
    public class ErrorResponseModel
    {
        [JsonProperty("code")]
        public string StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }
    }
}