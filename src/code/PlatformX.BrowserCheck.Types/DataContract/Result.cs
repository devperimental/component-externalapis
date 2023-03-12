using Newtonsoft.Json;

namespace PlatformX.BrowserCheck.Types.DataContract
{
    public class Result
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message_code")]
        public string MessageCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
