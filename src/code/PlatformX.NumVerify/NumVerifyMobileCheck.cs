using PlatformX.Http.Behaviours;
using PlatformX.MobileCheck.Types;
using PlatformX.MobileCheck.Types.DataContract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlatformX.NumVerify
{
    public class NumVerifyMobileCheck : IMobileCheckProvider
    {
        private readonly NumVerifyConfiguration _configuration;
        private readonly IHttpRequestHelper _httpHelper;

        public NumVerifyMobileCheck(NumVerifyConfiguration configuration, IHttpRequestHelper httpHelper)
        {
            _configuration = configuration;
            _httpHelper = httpHelper ?? throw new ArgumentNullException("httpHelper", "httpHelper is null");
        }

        public async Task<MobileCheckResponse> CheckNumber(MobileCheckRequest request)
        {
            var urlFormat = _configuration.ApiUrl;
            if (string.IsNullOrEmpty(urlFormat))
            {
                throw new ArgumentNullException("urlFormat", "NumVerifyApiUrl is empty");
            }

            var apiKey = _configuration.ApiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey", "NumVerifyKeyName value is empty");
            }

            var url = string.Format(urlFormat, apiKey, request.MobileNumber);
            var data = string.Empty;
            var headers = new Dictionary<string, string> ();

            var response = await _httpHelper.SubmitRequest<MobileCheckResponse>(url, HttpMethod.Get, data, headers);

            return response;
        }
    }
}


