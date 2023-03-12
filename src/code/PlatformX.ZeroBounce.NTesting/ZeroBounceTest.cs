using Moq;
using Moq.Protected;
using NUnit.Framework;
using PlatformX.Http.Helper;
using PlatformX.MailboxCheck.Types.DataContract;
using PlatformX.Settings.Shared.Behaviours;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace PlatformX.ZeroBounce.NTesting
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestMailboxCheckValid()
        {
            var appSettings = new Mock<IPortalSettings>();

            var token = "ZEROBOUNCE-APIKEY"; // Environment.GetEnvironmentVariable("ZEROBOUNCE-APIKEY");

            appSettings.Setup(c => c.GetPortalSetting<bool>("ZeroBounceAllowFailRequest")).Returns(true);

            appSettings.Setup(c => c.GetPortalSetting<string>("ZeroBounceKeyName")).Returns("ZEROBOUNCE-APIKEY");
            appSettings.Setup(c => c.GetPortalSetting<string>("ZeroBounceApiUrl")).Returns("https://api.zerobounce.net/v2/validate?api_key={0}&email={1}&ip_address=");

            appSettings.Setup(c => c.GetSecretString("ZEROBOUNCE-APIKEY")).Returns(token);

            var request = new MailboxCheckRequest
            {
                EmailAddress = "farinella@gmail.com",
                IpAddress = ""
            };

            var responseData = "{ \"status\" : \"valid\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new ZeroBounceMailboxCheck(appSettings.Object, httpRequestHelper);

            var response = client.CheckEmailAddress(request);

            Assert.IsTrue(response.Status == "valid");
        }

        [Test]
        public void TestMailboxCheckDidYouMean()
        {
            var appSettings = new Mock<IPortalSettings>();

            var token = "ZEROBOUNCE-APIKEY"; // Environment.GetEnvironmentVariable("ZEROBOUNCE-APIKEY");

            appSettings.Setup(c => c.GetPortalSetting<string>("ZeroBounceKeyName")).Returns("ZEROBOUNCE-APIKEY");
            appSettings.Setup(c => c.GetPortalSetting<string>("ZeroBounceApiUrl")).Returns("https://api.zerobounce.net/v2/validate?api_key={0}&email={1}&ip_address=");

            appSettings.Setup(c => c.GetSecretString("ZEROBOUNCE-APIKEY")).Returns(token);

            var request = new MailboxCheckRequest
            {
                EmailAddress = "farinella@gnail.com",
                IpAddress = ""
            };

            var responseData = "{ \"did_you_mean\" : \"farinella@gmail.com\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new ZeroBounceMailboxCheck(appSettings.Object, httpRequestHelper);

            var response = client.CheckEmailAddress(request);

            Assert.IsTrue(!string.IsNullOrEmpty(response.DidYouMean));
        }

        [Test]
        public void TestMailboxCheckInvalid()
        {
            var appSettings = new Mock<IPortalSettings>();

            var token = "ZEROBOUNCE-APIKEY"; // Environment.GetEnvironmentVariable("ZEROBOUNCE-APIKEY");

            appSettings.Setup(c => c.GetPortalSetting<string>("ZeroBounceKeyName")).Returns("ZEROBOUNCE-APIKEY");
            appSettings.Setup(c => c.GetPortalSetting<string>("ZeroBounceApiUrl")).Returns("https://api.zerobounce.net/v2/validate?api_key={0}&email={1}&ip_address=");

            appSettings.Setup(c => c.GetSecretString("ZEROBOUNCE-APIKEY")).Returns(token);

            var request = new MailboxCheckRequest
            {
                EmailAddress = "farinella@gk.in",
                IpAddress = ""
            };

            var responseData = "{ \"status\" : \"invalid\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new ZeroBounceMailboxCheck(appSettings.Object, httpRequestHelper);

            var response = client.CheckEmailAddress(request);

            Assert.IsTrue(response.Status == "invalid");
        }

        public static Mock<IHttpClientFactory> CreateHttpClientFactoryMock(string content)
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(content)
            };

            // Create Client Handler
            var clientHandler = new Mock<DelegatingHandler>();

            clientHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage)
                .Verifiable();

            clientHandler.As<IDisposable>().Setup(s => s.Dispose());

            // Create Http Client
            var httpClient = new HttpClient(clientHandler.Object);

            // Create Http Factory
            var clientFactory = new Mock<IHttpClientFactory>(MockBehavior.Strict);

            clientFactory
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient)
                .Verifiable();

            return clientFactory;
        }
    }
}