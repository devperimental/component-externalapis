using Moq;
using Moq.Protected;
using NUnit.Framework;
using PlatformX.Fraudguard;
using PlatformX.Http.Helper;
using PlatformX.IpCheck.Types.DataContract;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using PlatformX.Settings.Shared.Behaviours;

namespace PlatformX.FraudGuard.NTesting
{
    public class FraudGuardTesting
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFraudguardCheck()
        {
            var appSettings = new Mock<IPortalSettings>();

            var userName = "FRAUDGUARD-APIUSERNAME"; // Environment.GetEnvironmentVariable("FRAUDGUARD-APIUSERNAME");
            var password = "FRAUDGUARD-APIPASSWORD"; //Environment.GetEnvironmentVariable("FRAUDGUARD-APIPASSWORD");

            appSettings.Setup(c => c.GetPortalSetting<string>("FraudguardApiUserNameKeyName")).Returns("FRAUDGUARD-APIUSERNAME");
            appSettings.Setup(c => c.GetPortalSetting<string>("FraudguardApiPasswordKeyName")).Returns("FRAUDGUARD-APIPASSWORD");
            appSettings.Setup(c => c.GetPortalSetting<string>("FraudguardApiUrlPattern")).Returns("https://api.fraudguard.io/ip/{0}");

            appSettings.Setup(c => c.GetSecretString("FRAUDGUARD-APIUSERNAME")).Returns(userName);
            appSettings.Setup(c => c.GetSecretString("FRAUDGUARD-APIPASSWORD")).Returns(password);

            var request = new IpCheckRequest
            {
                IpAddress = "147.135.36.62"
            };

            var responseData = "{ \"risk_level\" : \"low\", \"threat\" : \"minimal\", \"asn\" : \"123456\" }";
            var appLogger = new Mock<ILogger<HttpRequestHelper>>();
            var httpClientFactoryMock = CreateHttpClientFactoryMock(responseData);
            var httpRequestHelper = new HttpRequestHelper(httpClientFactoryMock.Object, appLogger.Object);

            var client = new FraudGuardIpCheck(appSettings.Object, httpRequestHelper);

            var response = client.CheckIp(request);

            Assert.IsTrue(!string.IsNullOrEmpty(response.RiskLevel));
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