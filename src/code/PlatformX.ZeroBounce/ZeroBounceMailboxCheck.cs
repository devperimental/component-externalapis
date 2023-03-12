using PlatformX.Http.Behaviours;
using PlatformX.MailboxCheck.Types.Behaviours;
using PlatformX.MailboxCheck.Types.DataContract;
using PlatformX.Settings.Shared.Behaviours;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PlatformX.ZeroBounce
{
    public class ZeroBounceMailboxCheck : IMailboxCheckProvider
    {
        private readonly IPortalSettings _appSettings;
        private readonly IHttpRequestHelper _httpHelper;
      
        public ZeroBounceMailboxCheck(IPortalSettings appSettings, IHttpRequestHelper httpHelper)
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

        public MailboxCheckResponse CheckEmailAddress(MailboxCheckRequest request)
        {
            var urlFormat = _appSettings.GetPortalSetting<string>("ZeroBounceApiUrl");
            if (string.IsNullOrEmpty(urlFormat))
            {
                throw new ArgumentNullException("urlFormat", "ZeroBounceApiUrl is empty");
            }

            var keyName = _appSettings.GetPortalSetting<string>("ZeroBounceKeyName");
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName", "ZeroBounceKeyName is empty");
            }

            var apiKey = _appSettings.GetSecretString(keyName);
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey", "ZeroBounceKeyName value is empty");
            }

            var url = string.Format(urlFormat, apiKey, request.EmailAddress, request.IpAddress);
            var data = string.Empty;
            var headers = new Dictionary<string, string>();

            var response = _httpHelper.SubmitRequest<MailboxCheckResponse>(url, HttpMethod.Get, data, headers);

            return response.Result;
        }
    }
}