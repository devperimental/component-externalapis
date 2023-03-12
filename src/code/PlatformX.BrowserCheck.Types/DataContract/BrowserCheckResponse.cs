using Newtonsoft.Json;

namespace PlatformX.BrowserCheck.Types.DataContract
{
    public class BrowserCheckResponse
    {
        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("parse")]
        public Parse Parse {get;set;}
    }
}
