using Newtonsoft.Json;
using PropertyChanged;

namespace XOCV.Models
{
    [ImplementPropertyChanged]
    public class LoginModel
    {
        [JsonProperty("usernameOrEmailAddress")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}