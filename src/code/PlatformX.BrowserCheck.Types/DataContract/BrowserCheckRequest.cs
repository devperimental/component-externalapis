using Newtonsoft.Json;

namespace PlatformX.BrowserCheck.Types.DataContract
{
    public class BrowserCheckRequest
    {
        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }
    }
}
