using Moq;
using Moq.Protected;
using NUnit.Framework;
using PlatformX.Http.Helper;
using PlatformX.MobileCheck.Types.DataContract;
using Microsoft.Extensions.Logging;

namespace PlatformX.NumVerify.NTesting
{
    public class NumVerifyTesting
    {
        private NumVerifyConfiguration _configuration;
        
        [SetUp]
        public void Setup()
        {
            _configuration = new NumVerifyConfiguration
            {
                ApiKey = "NUMVERIFY-APIKEY",
                ApiUrl = "http://apilayer.net/api/validate?access_key={0}&number={1}&country_code=&format=1"
            };
        }

        [Test]
        public async Task TestMobileNumberCheck()
        {
            var request = new MobileCheckRequest
            {
                MobileNumber = "61422100100"
            };

            var responseData = "{ \"valid\" : \"true\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new NumVerifyMobileCheck(_configuration, httpRequestHelper);

            var response = await client.CheckNumber(request);

            Assert.IsTrue(response.Valid);
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