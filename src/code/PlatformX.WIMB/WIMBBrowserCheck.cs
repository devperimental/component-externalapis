using Newtonsoft.Json;
using PlatformX.BrowserCheck.Types;
using PlatformX.BrowserCheck.Types.DataContract;
using PlatformX.Http.Behaviours;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlatformX.WIMB
{
    public class WIMBBrowserCheck : IBrowserCheckProvider
    {
        private readonly WIMBConfiguration _configuration;
        private readonly IHttpRequestHelper _httpHelper;

        public WIMBBrowserCheck(WIMBConfiguration configuration, IHttpRequestHelper httpHelper)
        {
            _configuration = configuration;
            _httpHelper = httpHelper ?? throw new ArgumentNullException("httpHelper", "httpHelper is null");
        }

        public async Task<BrowserCheckResponse> CheckUserAgent(BrowserCheckRequest request)
        {
            var url = _configuration.ApiUrl;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url", "WIMBApiUrl is empty");
            }

            var apiKey = _configuration.ApiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey", "WIMBKeyName value is empty");
            }

            var data = JsonConvert.SerializeObject(request);
            var headers = new Dictionary<string, string> {
                { "x-api-key", apiKey }
            };

            var response = await _httpHelper.SubmitRequest<BrowserCheckResponse>(url, HttpMethod.Post, data, headers);

            return response;
        }
    }
}