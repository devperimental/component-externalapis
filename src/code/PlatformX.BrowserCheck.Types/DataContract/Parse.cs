using Newtonsoft.Json;

namespace PlatformX.BrowserCheck.Types.DataContract
{
    public class Parse
    {
        [JsonProperty("simple_software_string")]
        public string SimpleSoftwareString { get; set; }
        [JsonProperty("simple_sub_description_string")]
        public string SimpleSubDescriptionString { get; set; }
        [JsonProperty("simple_operating_platform_string")]
        public string SimpleOperatingPlatformString { get; set; }
        [JsonProperty("software")]
        public string Software { get; set; }
        [JsonProperty("software_name")]
        public string SoftwareName { get; set; }
        [JsonProperty("software_name_code")]
        public string SoftwareNameCode { get; set; }
        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; }
        [JsonProperty("software_version_full")]
        public string[] SoftwareVersionFull { get; set; }
        [JsonProperty("operating_system")]
        public string OperatingSystem { get; set; }
        [JsonProperty("operating_system_name")]
        public string OperatingSystemName { get; set; }
        [JsonProperty("operating_system_name_code")]
        public string OperatingSystemNameCode { get; set; }
        [JsonProperty("operating_system_flavour")]
        public string OperatingSystemFlavour { get; set; }
        [JsonProperty("operating_system_flavour_code")]
        public string OperatingSystemFlavourCode { get; set; }
        [JsonProperty("operating_system_version")]
        public string OperatingSystemVersion { get; set; }
        [JsonProperty("operating_system_version_full")]
        public string[] OperatingSystemVersionFull { get; set; }
        [JsonProperty("is_abusive")]
        public bool IsAbusive { get; set; }
        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }

    }
}
