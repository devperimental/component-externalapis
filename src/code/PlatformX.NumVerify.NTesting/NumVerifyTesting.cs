using Moq;
using Moq.Protected;
using NUnit.Framework;
using PlatformX.Http.Helper;
using PlatformX.MobileCheck.Types.DataContract;
using Microsoft.Extensions.Logging;
using PlatformX.Settings.Shared.Behaviours;

namespace PlatformX.NumVerify.NTesting
{
    public class NumVerifyTesting
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMobileNumberCheck()
        {
            var appSettings = new Mock<IPortalSettings>();

            var token = "NUMVERIFY-APIKEY"; Environment.GetEnvironmentVariable("NUMVERIFY-APIKEY");

            appSettings.Setup(c => c.GetPortalSetting<string>("NumVerifyKeyName")).Returns("NUMVERIFY-APIKEY");
            appSettings.Setup(c => c.GetPortalSetting<string>("NumVerifyApiUrl")).Returns("http://apilayer.net/api/validate?access_key={0}&number={1}&country_code=&format=1");

            appSettings.Setup(c => c.GetSecretString("NUMVERIFY-APIKEY")).Returns(token);

            var request = new MobileCheckRequest
            {
                MobileNumber = "61422100100"
            };

            var responseData = "{ \"valid\" : \"true\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new NumVerifyMobileCheck(appSettings.Object, httpRequestHelper);

            var response = client.CheckNumber(request);

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