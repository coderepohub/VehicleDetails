using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Net;
using VehicleDetails.API.Middlewares;
using VehicleDetails.DomainModel;
using VehicleDetails.DomainModel.Enums;

namespace VehicleDetails.UnitTests.MiddlewareTests
{
    public class GlobalExceptionHandlerTests
    {
        private readonly Mock<ILogger<GlobalExceptionHandler>> _mockLogger;

        public GlobalExceptionHandlerTests()
        {
            _mockLogger = new Mock<ILogger<GlobalExceptionHandler>>();
        }

        [Fact]
        public async Task InvokeAsync_Should_Handle_Exception_And_Write_Response()
        {
            // Arrange
            string exceptionMessage = "Test the global exception";
            var middleware = new GlobalExceptionHandler(next: (innerHttpContext) =>
            {
                throw new Exception(exceptionMessage);
            }, _mockLogger.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            context.Response.ContentType.Should().Be("application/json");


            var expectedResponse = new VehicleDetailsApiResponse<VehicleDetailsApiError>
            {
                Status = HttpStatus.Error.ToString(),
                Data = new List<VehicleDetailsApiError>
                  {
                      new VehicleDetailsApiError
                      {
                          Message = "An error occurred while processing your request.",
                          Details = exceptionMessage
                      }
                  }
            };

            //Taking the body seek to begnining to read
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var actualResponse = await reader.ReadToEndAsync();

            actualResponse.Should().Be(JsonConvert.SerializeObject(expectedResponse));
        }

    }
}
