using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleDetails.API.Middlewares;
using VehicleDetails.DomainModel.Options;

namespace VehicleDetails.UnitTests.MiddlewareTests
{
    public class HttpClientRegistrationTests
    {
        [Fact]
        public void AddCryptoHttpClient_Should_Register_HttpClient_With_CryptoApiOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            var cryptoApiOptions = new RDWApiOption
            {
                BaseUrl = "https://example.com"
            };
            services.Configure<RDWApiOption>(options =>
            {
                options.BaseUrl = cryptoApiOptions.BaseUrl;
            });

            // Act
            services.AddVehicleDetailsHttpClient();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(HttpClientSetting.ClientName);
            httpClient.BaseAddress.Should().Be(new Uri(cryptoApiOptions.BaseUrl));
            httpClient.DefaultRequestHeaders.Accept.Should().ContainSingle(header => header.MediaType == "application/json");
        }
    }
}
