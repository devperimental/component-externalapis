using Newtonsoft.Json;
using PlatformX.BrowserCheck.Types;
using PlatformX.BrowserCheck.Types.DataContract;
using PlatformX.Http.Behaviours;
using PlatformX.Settings.Shared.Behaviours;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PlatformX.WIMB
{
    public class WIMBBrowserCheck : IBrowserCheckProvider
    {
        private readonly IPortalSettings _appSettings;
        private readonly IHttpRequestHelper _httpHelper;

        public WIMBBrowserCheck(IPortalSettings appSettings, IHttpRequestHelper httpHelper)
        {
            if (appSettings == null)
            {
                throw new ArgumentNullException("appSettings", "appSettings is null");
            }

            if (httpHelper == null)
            {
                throw new ArgumentNullException("httpHelper", "httpHelper is null");
            }

            _appSettings = appSettings;
            _httpHelper = httpHelper;
        }

        public BrowserCheckResponse CheckUserAgent(BrowserCheckRequest request)
        {
            var url = _appSettings.GetPortalSetting<string>("WIMBApiUrl");
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url", "WIMBApiUrl is empty");
            }

            var keyName = _appSettings.GetPortalSetting<string>("WIMBKeyName");
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName", "WIMBKeyName is empty");
            }

            var apiKey = _appSettings.GetSecretString(keyName);
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey", "WIMBKeyName value is empty");
            }

            var data = JsonConvert.SerializeObject(request);
            var headers = new Dictionary<string, string> {
                { "x-api-key", apiKey }
            };

            var response = _httpHelper.SubmitRequest<BrowserCheckResponse>(url, HttpMethod.Post, data, headers);

            return response.Result;
        }
    }
}