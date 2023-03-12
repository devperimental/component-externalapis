﻿using Newtonsoft.Json;
using System;

namespace PlatformX.MailboxCheck.Types.DataContract
{
    public class MailboxCheckResponse
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("sub_status")]
        public string SubStatus { get; set; }
        [JsonProperty("free_email")]
        public bool FreeEmail { get; set; }
        [JsonProperty("did_you_mean")]
        public string DidYouMean { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("domain_age_days")]
        public int? DomainAgeDays { get; set; }
        [JsonProperty("smtp_provider")]
        public string SmtpProvider { get; set; }
        [JsonProperty("mx_found")]
        public bool MxFound { get; set; }
        [JsonProperty("mx_record")]
        public string MxRecord { get; set; }
        [JsonProperty("firstname")]
        public string FirstName { get; set; }
        [JsonProperty("lastname")]
        public string LastName { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("zipcode")]
        public string ZipCode { get; set; }
        [JsonProperty("processed_at")]
        public DateTime? ProcessedAt { get; set; }
    }
}
