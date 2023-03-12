using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PlatformX.BrowserCheck.Types.DataContract;
using PlatformX.Http.Helper;
using PlatformX.Settings.Shared.Behaviours;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace PlatformX.WIMB.NTesting
{
    public class WIMBTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void TestWIMBCheck()
        {
            var appSettings = new Mock<IPortalSettings>();

            var token = "WIMB-APIKEY"; // Environment.GetEnvironmentVariable("WIMB-APIKEY");

            appSettings.Setup(c => c.GetPortalSetting<string>("WIMBKeyName")).Returns("WIMB-APIKEY");
            appSettings.Setup(c => c.GetPortalSetting<string>("WIMBApiUrl")).Returns("https://api.whatismybrowser.com/api/v2/user_agent_parse");
            appSettings.Setup(c => c.GetSecretString("WIMB-APIKEY")).Returns(token);

            var request = new BrowserCheckRequest
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36"
            };

            var responseData = "{ \"result\": { \"code\" : \"success\", \"message_code\" : \"success\", \"message\" : \"success\" }, \"parse\": { \"user_agent\" : \"internet explorer\" } }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();

            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);

            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);
            
            var client = new WIMBBrowserCheck(appSettings.Object, httpRequestHelper);

            var response = client.CheckUserAgent(request);

            Assert.IsTrue(response.Result.Code == "success");
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