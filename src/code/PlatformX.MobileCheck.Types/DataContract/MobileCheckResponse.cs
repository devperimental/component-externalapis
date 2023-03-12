using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformX.MobileCheck.Types.DataContract
{
    public class MobileCheckResponse
    {
        [JsonProperty("valid")]
        public bool Valid { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("local_format")]
        public string LocalFormat { get; set; }
        [JsonProperty("international_format")]
        public string InternationalFormat { get; set; }
        [JsonProperty("country_prefix")]
        public string CountryPrefix { get; set; }
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        [JsonProperty("country_name")]
        public string CountryName { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("carrier")]
        public string Carrier { get; set; }
        [JsonProperty("line_type")]
        public string LineType { get; set; }
    }
}
