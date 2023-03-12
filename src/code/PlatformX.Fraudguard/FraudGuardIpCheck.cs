using PlatformX.Http.Behaviours;
using PlatformX.IpCheck.Types;
using PlatformX.IpCheck.Types.DataContract;
using PlatformX.Settings.Shared.Behaviours;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PlatformX.Fraudguard
{
    public class FraudGuardIpCheck : IIpCheckProvider
    {
        private readonly IPortalSettings _appSettings;
        private readonly IHttpRequestHelper _httpHelper;
        
        public FraudGuardIpCheck(IPortalSettings appSettings, IHttpRequestHelper httpHelper)
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

        public IpCheckResponse CheckIp(IpCheckRequest request)
        {
            var urlPattern = _appSettings.GetPortalSetting<string>("FraudguardApiUrlPattern");
            if (string.IsNullOrEmpty(urlPattern))
            {
                throw new ArgumentNullException("urlPattern", "FraudguardApiUrlPattern is empty");
            }

            var userNameKeyName = _appSettings.GetPortalSetting<string>("FraudguardApiUserNameKeyName");
            if (string.IsNullOrEmpty(userNameKeyName))
            {
                throw new ArgumentNullException("userNameKeyName", "FraudguardApiUserNameKeyName is empty");
            }

            var passwordKeyName = _appSettings.GetPortalSetting<string>("FraudguardApiPasswordKeyName");
            if (string.IsNullOrEmpty(passwordKeyName))
            {
                throw new ArgumentNullException("passwordKeyName", "FraudguardApiUserNameKeyName is empty");
            }

            var userName = _appSettings.GetSecretString(userNameKeyName);
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName", "value is empty");
            }

            var password = _appSettings.GetSecretString(passwordKeyName);
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password", "value is empty");
            }

            var url = string.Format(urlPattern, request.IpAddress);
            var token = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(userName + ":" + password));
            var headers = new Dictionary<string, string> {
                { "Authorization", $"Basic {token}" }
            };

            var response = _httpHelper.SubmitRequest<IpCheckResponse>(url, HttpMethod.Get, string.Empty, headers);

            return response.Result;
        }
    }
}
