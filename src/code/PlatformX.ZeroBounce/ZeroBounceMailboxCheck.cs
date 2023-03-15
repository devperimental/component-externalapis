using PlatformX.Http.Behaviours;
using PlatformX.MailboxCheck.Types.Behaviours;
using PlatformX.MailboxCheck.Types.DataContract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlatformX.ZeroBounce
{
    public class ZeroBounceMailboxCheck : IMailboxCheckProvider
    {
        private readonly ZeroBounceConfiguration _configuration;
        private readonly IHttpRequestHelper _httpHelper;

        public ZeroBounceMailboxCheck(ZeroBounceConfiguration configuration, IHttpRequestHelper httpHelper)
        {
            _configuration = configuration;
            _httpHelper = httpHelper ?? throw new ArgumentNullException("httpHelper", "httpHelper is null");
        }

        public async Task<MailboxCheckResponse> CheckEmailAddress(MailboxCheckRequest request)
        {
            var urlFormat = _configuration.ApiUrl;
            if (string.IsNullOrEmpty(urlFormat))
            {
                throw new ArgumentNullException("urlFormat", "ZeroBounceApiUrl is empty");
            }

            var apiKey = _configuration.ApiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey", "ZeroBounceKeyName value is empty");
            }

            var url = string.Format(urlFormat, apiKey, request.EmailAddress, request.IpAddress);
            var data = string.Empty;
            var headers = new Dictionary<string, string>();

            var response = await _httpHelper.SubmitRequest<MailboxCheckResponse>(url, HttpMethod.Get, data, headers);

            return response;
        }
    }
}