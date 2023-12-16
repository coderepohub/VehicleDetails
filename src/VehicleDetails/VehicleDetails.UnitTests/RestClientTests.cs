using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel.Options;
using VehicleDetails.DomainModel;
using VehicleDetails.HttpConnector;

namespace VehicleDetails.UnitTests
{
    public class RestClientTests
    {
        private readonly Mock<IHttpClientProvider> _mockHttpClientProvider;
        private readonly Mock<IOptions<RDWApiOption>> _mockrdwApiOption;
        private readonly Mock<ILogger<RestClient>> _mockLogger;
        private readonly IRestClient _restClient;
        private readonly RDWApiOption _rdwApiOptions;
        public RestClientTests()
        {
            _mockHttpClientProvider = new Mock<IHttpClientProvider>();
            _mockrdwApiOption = new Mock<IOptions<RDWApiOption>>();
            _rdwApiOptions = new RDWApiOption
            {
                BaseUrl = "https://mock.com"
            };
            _mockrdwApiOption.Setup(o => o.Value).Returns(_rdwApiOptions);
            _mockLogger = new Mock<ILogger<RestClient>>();

            _restClient = new RestClient(_mockHttpClientProvider.Object, _mockrdwApiOption.Object, _mockLogger.Object);
        }


        #region Test for Constructor

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            // Act
            Action act = () => new RestClient(_mockHttpClientProvider.Object, null, _mockLogger.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*options*");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientProviderIsNull()
        {
            // Act
            Action act = () => new RestClient(null, _mockrdwApiOption.Object, _mockLogger.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*httpClientProvider*");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            // Act
            Action act = () => new RestClient(_mockHttpClientProvider.Object, _mockrdwApiOption.Object, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
        }
        #endregion


        #region Test for GetRDWVehicleDetails method

        [Fact]
        public async Task GetRDWVehicleDetails_ShouldThrowArgumentException_WhenBothKentekenAndMerkIsNull()
        {
            // Act
            Func<Task> act = () => _restClient.GetRDWVehicleDetails(null, null);

            // Assert
            await act.Should().ThrowAsync<HttpResponseException>();
            _mockLogger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"licenseplate")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Theory]
        [InlineData("HG880V", "Ford")]
        [InlineData("FH651T", "BMW")]
        [InlineData("FH651T", "")]
        public async Task GetRDWVehicleDetails_ShouldReturnRDWApiVehicleDataResponse_WhenHttpResponseIsSuccessful(string licensePlate, string brand)
        {
            // Arrange
            var expectedResponse = new List<RDWApiVehicleDataResponse>
            {
                new RDWApiVehicleDataResponse
                {
                     Kenteken = licensePlate,
                     Merk = brand,
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResponse);
            var expectedUri = $"?kenteken={licensePlate}&merk={brand}";
            var hasQuery = false;
            if (!string.IsNullOrWhiteSpace(licensePlate))
            {
                expectedUri = $"?kenteken={licensePlate}";
                hasQuery = true;
            }
            if (!string.IsNullOrWhiteSpace(brand))
            {
                expectedUri += hasQuery ? "&" : "?";
                expectedUri += $"merk={brand}";
            }
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedJson)
            };
            _mockHttpClientProvider.Setup(h => h.GetJsonAsync(expectedUri)).ReturnsAsync(httpResponseMessage);

            // Act
            var actualResponse = await _restClient.GetRDWVehicleDetails(licensePlate, brand);

            // Assert
            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }


        [Fact]
        public async Task GetCryptoCurrencyCodesAsync_ShouldThrowHttpResponseException_WhenHttpResponseMessageIsNotOk()
        {
            // Arrange
            var expectedStatusCode = HttpStatusCode.BadRequest;
            var expectedMessage = "Invalid request";
            var httpResponseMessage = new HttpResponseMessage(expectedStatusCode)
            {
                Content = new StringContent(expectedMessage)
            };
            _mockHttpClientProvider.Setup(h => h.GetJsonAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

            // Act
            Func<Task> act = () => _restClient.GetRDWVehicleDetails("Invalidkenteken", "merk");

            // Assert
            await act.Should().ThrowAsync<HttpResponseException>().Where(e => e.StatusCode == (int)HttpStatusCode.BadRequest);

        }

        [Fact]
        public async Task GetCryptoCurrencyCodesAsync_ShouldThrowHttpResponseException_WhenHttpResponseMessageIsNull()
        {
            // Arrange
            _mockHttpClientProvider.Setup(h => h.GetJsonAsync(It.IsAny<string>())).ReturnsAsync((HttpResponseMessage)null);

            // Act
            Func<Task> act = () => _restClient.GetRDWVehicleDetails("InvalidData", "InvalidData");

            // Assert
            await act.Should().ThrowAsync<HttpResponseException>().Where(e => e.StatusCode == (int)HttpStatusCode.ServiceUnavailable);
        }

        [Fact]
        public async Task GetCryptoCurrencyCodesAsync_ShouldThrowHttpResponseException_WhenHttpResponseContentIsNull()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = null
            };
            _mockHttpClientProvider.Setup(h => h.GetJsonAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

            // Act
            Func<Task> act = () => _restClient.GetRDWVehicleDetails("InvalidData", "InvalidData");

            // Assert
            await act.Should().ThrowAsync<HttpResponseException>().Where(e => e.StatusCode == (int)HttpStatusCode.BadGateway && e.Value.ToString().Contains("no response")); ;
        }

        #endregion

    }
}
