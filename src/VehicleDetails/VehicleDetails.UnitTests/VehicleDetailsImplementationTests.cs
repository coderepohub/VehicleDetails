using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel;
using VehicleDetails.Implementation;

public class VehicleDetailsImplementationTests
{
    private readonly Mock<ILogger<VehicleDetailsImplementation>> _loggerMock;
    private readonly Mock<ICachingService> _mockCahingService;
    private readonly Mock<IRestClient> _mockRestClient;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IVehicleDetailsImplementation _vehicleDetailsImplementation;
    public VehicleDetailsImplementationTests()
    {
        _loggerMock = new Mock<ILogger<VehicleDetailsImplementation>>();
        _mockCahingService = new Mock<ICachingService>();
        _mockRestClient = new Mock<IRestClient>();
        _mockMapper = new Mock<IMapper>();
        _vehicleDetailsImplementation = new VehicleDetailsImplementation(
            _mockRestClient.Object,
            _loggerMock.Object,
            _mockMapper.Object,
            _mockCahingService.Object);
    }

    [Fact]
    public async Task GetBasicVehicleDetails_ShouldReturnMappedResults_WhenCachingServiceHasNoData()
    {
        // Arrange
        var vehicleDetailsQuery = new VehicleDetailsQuery { Kenteken = "ABC123", Merk = "Ford" };

        _mockCahingService.Setup(x => x.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<RDWApiVehicleDataResponse>>>>()))
            .ReturnsAsync(() => null); // Simulate no data in the cache

        _mockRestClient.Setup(x => x.GetRDWVehicleDetails(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new List<RDWApiVehicleDataResponse>());


        // Act
        var result = await _vehicleDetailsImplementation.GetBasicVehiclDetailsAsync(vehicleDetailsQuery);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _mockMapper.Verify(x => x.Map<IEnumerable<BasicVehicleDetail>>(It.IsAny<IEnumerable<RDWApiVehicleDataResponse>>()), Times.Once);
    }

    [Fact]
    public async Task GetBasicVehicleDetails_ShouldReturnCachedResults_WhenCachingServiceHasData()
    {
        // Arrange
        var vehicleDetailsQuery = new VehicleDetailsQuery { Kenteken = "ABC123", Merk = "Toyota" };

        var cachedData = new List<RDWApiVehicleDataResponse>
        {
            new RDWApiVehicleDataResponse
            {
                 Kenteken = vehicleDetailsQuery.Kenteken,
                 Merk = vehicleDetailsQuery.Merk,
            }
        };

        _mockCahingService.Setup(x => x.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<RDWApiVehicleDataResponse>>>>()))
            .ReturnsAsync(cachedData);

        // Act
        var result = await _vehicleDetailsImplementation.GetBasicVehiclDetailsAsync(vehicleDetailsQuery);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_mockMapper.Object.Map<IEnumerable<BasicVehicleDetail>>(cachedData));
        _mockRestClient.Verify(x => x.GetRDWVehicleDetails(vehicleDetailsQuery.Kenteken,vehicleDetailsQuery.Merk), Times.Never);
    }

}
