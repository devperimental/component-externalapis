using Moq;
using Moq.Protected;
using NUnit.Framework;
using PlatformX.Http.Helper;
using PlatformX.MailboxCheck.Types.DataContract;
using Microsoft.Extensions.Logging;

namespace PlatformX.ZeroBounce.NTesting
{
    [TestFixture]
    public class Tests
    {
        private ZeroBounceConfiguration _configuration;
        [SetUp]
        public void Setup()
        {
            _configuration = new ZeroBounceConfiguration
            {
                ApiKey = "ZEROBOUNCE-APIKEY",
                ApiUrl = "https://api.zerobounce.net/v2/validate?api_key={0}&email={1}&ip_address="
            };
        }

        [Test]
        public async Task TestMailboxCheckValid()
        {
            var request = new MailboxCheckRequest
            {
                EmailAddress = "farinella@gmail.com",
                IpAddress = ""
            };

            var responseData = "{ \"status\" : \"valid\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new ZeroBounceMailboxCheck(_configuration, httpRequestHelper);

            var response = await client.CheckEmailAddress(request);

            Assert.IsTrue(response.Status == "valid");
        }

        [Test]
        public async Task TestMailboxCheckDidYouMean()
        {
            var request = new MailboxCheckRequest
            {
                EmailAddress = "farinella@gnail.com",
                IpAddress = ""
            };

            var responseData = "{ \"did_you_mean\" : \"farinella@gmail.com\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new ZeroBounceMailboxCheck(_configuration, httpRequestHelper);

            var response = await client.CheckEmailAddress(request);

            Assert.IsTrue(!string.IsNullOrEmpty(response.DidYouMean));
        }

        [Test]
        public async Task TestMailboxCheckInvalid()
        {
            var request = new MailboxCheckRequest
            {
                EmailAddress = "farinella@gk.in",
                IpAddress = ""
            };

            var responseData = "{ \"status\" : \"invalid\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new ZeroBounceMailboxCheck(_configuration, httpRequestHelper);

            var response = await client.CheckEmailAddress(request);

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