using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using VehicleDetails.API;
using VehicleDetails.DomainModel;
using VehicleDetails.DomainModel.Options;

namespace VehicleDtails.IntegrationTests
{
    public class VehicleDetailsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _authKey;

        public VehicleDetailsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Adjust the path accordingly
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = configBuilder.Build();
            _authKey = configuration["ApiAuth:ApiKey"];
        }

        /// <summary>
        /// When only Kenteken is passed in the request, should return ok response
        /// </summary>
        [Fact]
        public async Task Post_ValidRequest_WithOnlyKenteken_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var kenteken = "HG880V";

            // Add the API key to the request headers
            client.DefaultRequestHeaders.Add(ApiAuthenticationOptions.AuthKeyName, _authKey);

            // Vehicle Details Query
            var vehicleDetailsQuery = new VehicleDetailsQuery
            {
                Kenteken = kenteken
            };

            var content = new StringContent(JsonConvert.SerializeObject(vehicleDetailsQuery), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/VehicleDetails", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<VehicleDetailsApiResponse<BasicVehicleDetail>>(responseBody);

            responseObject.Should().NotBeNull();
            responseObject.Status.Should().Be("Success");
            responseObject.Data.Should().NotBeNull();
            responseObject.Data.Should().NotBeEmpty();
            responseObject.Data.FirstOrDefault().LicensePlate.Should().Be(kenteken);
        }


        /// <summary>
        /// When only Merk is passed in the request, should return ok response
        /// </summary>
        [Fact]
        public async Task Post_ValidRequest_WithOnlyMerk_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var model = "FORD";

            // Add the API key to the request headers
            client.DefaultRequestHeaders.Add(ApiAuthenticationOptions.AuthKeyName, _authKey);

            // Vehicle Details Query
            var vehicleDetailsQuery = new VehicleDetailsQuery
            {
                Merk = model
            };

            var content = new StringContent(JsonConvert.SerializeObject(vehicleDetailsQuery), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/VehicleDetails", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<VehicleDetailsApiResponse<BasicVehicleDetail>>(responseBody);

            responseObject.Should().NotBeNull();
            responseObject.Status.Should().Be("Success");
            responseObject.Data.Should().NotBeNull();
            responseObject.Data.Should().NotBeEmpty();
            responseObject.Data.FirstOrDefault().Model.Should().Be(model);

        }

        /// <summary>
        /// When both Kenteken and Merk is passed in the request, should return ok response
        /// </summary>
        [Fact]
        public async Task Post_ValidRequest_WithBothMerkAndKenteken_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var model = "FORD";
            var kenteken = "HG880V";

            // Add the API key to the request headers
            client.DefaultRequestHeaders.Add(ApiAuthenticationOptions.AuthKeyName, _authKey);

            // Vehicle Details Query
            var vehicleDetailsQuery = new VehicleDetailsQuery
            {
                Kenteken = kenteken,
                Merk = model
            };

            var content = new StringContent(JsonConvert.SerializeObject(vehicleDetailsQuery), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/VehicleDetails", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<VehicleDetailsApiResponse<BasicVehicleDetail>>(responseBody);

            responseObject.Should().NotBeNull();
            responseObject.Status.Should().Be("Success");
            responseObject.Data.Should().NotBeNull();
            responseObject.Data.Should().NotBeEmpty();
            responseObject.Data.FirstOrDefault().LicensePlate.Should().Be(kenteken);
            responseObject.Data.FirstOrDefault().Model.Should().Be(model);

        }

        /// <summary>
        /// Should return bad request when both kenteken and meldcode is not passed.
        /// </summary>
        [Fact]
        public async Task Post_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Add the API key to the request headers
            client.DefaultRequestHeaders.Add(ApiAuthenticationOptions.AuthKeyName, _authKey);

            // Set an invalid vehicleDetailsQuery
            var vehicleDetailsQuery = new VehicleDetailsQuery();

            var content = new StringContent(JsonConvert.SerializeObject(vehicleDetailsQuery), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/VehicleDetails", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }
    }
}