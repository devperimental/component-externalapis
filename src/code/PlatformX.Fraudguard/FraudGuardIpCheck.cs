using PlatformX.Http.Behaviours;
using PlatformX.IpCheck.Types;
using PlatformX.IpCheck.Types.DataContract;
using PlatformX.ZeroBounce;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlatformX.Fraudguard
{
    public class FraudGuardIpCheck : IIpCheckProvider
    {
        private readonly FraudGuardConfiguration _configuration;
        private readonly IHttpRequestHelper _httpHelper;
        
        public FraudGuardIpCheck(FraudGuardConfiguration configuration, IHttpRequestHelper httpHelper)
        {
            _configuration = configuration;
            _httpHelper = httpHelper ?? throw new ArgumentNullException("httpHelper", "httpHelper is null");
        }

        public async Task<IpCheckResponse> CheckIp(IpCheckRequest request)
        {
            var urlPattern = _configuration.UrlPattern;
            if (string.IsNullOrEmpty(urlPattern))
            {
                throw new ArgumentNullException("urlPattern", "FraudguardApiUrlPattern is empty");
            }

            var userName = _configuration.UserName;
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName", "value is empty");
            }

            var password = _configuration.Password;
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password", "value is empty");
            }

            var url = string.Format(urlPattern, request.IpAddress);
            var token = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));
            var headers = new Dictionary<string, string> {
                { "Authorization", $"Basic {token}" }
            };

            var response = await _httpHelper.SubmitRequest<IpCheckResponse>(url, HttpMethod.Get, string.Empty, headers);

            return response;
        }
    }
}
