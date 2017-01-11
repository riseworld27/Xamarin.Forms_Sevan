using Newtonsoft.Json;

namespace XOCV.Models.ResponseModels
{
    public class LoginResponseModel
    {
        [JsonProperty("result")]
        public string Token { get; set; }

        [JsonProperty("error")]
        public ErrorResponseModel Error { get; set; }
    }
}