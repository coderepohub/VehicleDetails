using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleDetails.DomainModel;
using VehicleDetails.Implementation.Mappers;

namespace VehicleDetails.UnitTests
{
    public class BasicVehicleDetailsMapperProfileTests
    {
        [Fact]
        public void Mapping_Configuration_Is_Valid()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BasicVehicleDetailsMapperProfile>();
            });

            // Act & Assert
            configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_RDWVehicleDetailsResponse_To_BasicVehicleDetail_Is_Valid()
        {
            // Arrange
            var rDWApiVehicleDataResponse = new RDWApiVehicleDataResponse
            {
                Kenteken = "Kenteken",
                Handelsbenaming = "Brand",
                Merk = "Merk",
                DatumEersteToelating = "20231211"

            };

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BasicVehicleDetailsMapperProfile>();
            });

            var mapper = configuration.CreateMapper();

            // Act
            var result = mapper.Map<BasicVehicleDetail>(rDWApiVehicleDataResponse);

            // Assert
            result.Should().NotBeNull();
            result.LicensePlate.Should().Be(rDWApiVehicleDataResponse.Kenteken);
            result.Model.Should().Be(rDWApiVehicleDataResponse.Merk);
            result.YearOfManufacture.ToString("yyyyMMdd").Should().Be(rDWApiVehicleDataResponse.DatumEersteToelating);
        }
    }
}
