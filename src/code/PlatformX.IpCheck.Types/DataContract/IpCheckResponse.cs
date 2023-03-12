using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformX.IpCheck.Types.DataContract
{
    public class IpCheckResponse
    {
        [JsonProperty("isocode")]
        public string IsoCode { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("state_code")]
        public string StateCode { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
        [JsonProperty("latitude")]
        public string Latitude { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        [JsonProperty("connection_type")]
        public string ConnectionType { get; set; }
        [JsonProperty("asn")]
        public string Asn { get; set; }
        [JsonProperty("asn_organization")]
        public string AsnOrganization { get; set; }
        [JsonProperty("isp")]
        public string Isp { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("discover_date")]
        public string DiscoverDate { get; set; }
        [JsonProperty("threat")]
        public string Threat { get; set; }
        [JsonProperty("risk_level")]
        public string RiskLevel { get; set; }
    }
}
