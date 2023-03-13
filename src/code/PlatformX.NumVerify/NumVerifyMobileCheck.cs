using PlatformX.Http.Behaviours;
using PlatformX.MobileCheck.Types;
using PlatformX.MobileCheck.Types.DataContract;
using PlatformX.Settings.Shared.Behaviours;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PlatformX.NumVerify
{
    public class NumVerifyMobileCheck : IMobileCheckProvider
    {
        private readonly IPortalSettings _appSettings;
        private readonly IHttpRequestHelper _httpHelper;
       
        public NumVerifyMobileCheck(IPortalSettings appSettings, IHttpRequestHelper httpHelper)
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

        public MobileCheckResponse CheckNumber(MobileCheckRequest request)
        {
            var urlFormat = _appSettings.GetPortalSetting<string>("NumVerifyApiUrl");
            if (string.IsNullOrEmpty(urlFormat))
            {
                throw new ArgumentNullException("urlFormat", "NumVerifyApiUrl is empty");
            }

            var keyName = _appSettings.GetPortalSetting<string>("NumVerifyKeyName");
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName", "NumVerifyKeyName is empty");
            }

            var apiKey = _appSettings.GetSecretString(keyName);
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey", "NumVerifyKeyName value is empty");
            }

            var url = string.Format(urlFormat, apiKey, request.MobileNumber);
            var data = string.Empty;
            var headers = new Dictionary<string, string> ();

            var response = _httpHelper.SubmitRequest<MobileCheckResponse>(url, HttpMethod.Get, data, headers);

            return response.Result;
        }
    }
}


