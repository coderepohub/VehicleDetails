using AutoMapper;
using System.Globalization;
using VehicleDetails.DomainModel;

namespace VehicleDetails.Implementation.Mappers
{
    public class BasicVehicleDetailsMapperProfile : Profile
    {
        public BasicVehicleDetailsMapperProfile()
        {
            CreateMap<RDWApiVehicleDataResponse, BasicVehicleDetail>()
                .ForMember(d => d.LicensePlate, o => o.MapFrom(s => s.Kenteken))
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Handelsbenaming))
                .ForMember(d => d.Model, o => o.MapFrom(s => s.Merk))
                .ForMember(d => d.YearOfManufacture, o => o.Ignore())
                .AfterMap((source, dest) =>
                {
                    dest.YearOfManufacture = ConvertToDateTime(source.DatumEersteToelating);
                });
        }

        private DateTime ConvertToDateTime(string v)
        {
            return DateTime.ParseExact(v, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
    }
}
