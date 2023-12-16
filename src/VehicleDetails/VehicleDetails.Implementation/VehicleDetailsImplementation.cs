using AutoMapper;
using Microsoft.Extensions.Logging;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel;

namespace VehicleDetails.Implementation
{
    public class VehicleDetailsImplementation : IVehicleDetailsImplementation
    {
        private readonly IRestClient _restClient;
        private readonly ILogger<VehicleDetailsImplementation> _logger;
        private readonly IMapper _mapper;
        private readonly ICachingService _cachingService;

        #region Constructor
        public VehicleDetailsImplementation(IRestClient restClient, ILogger<VehicleDetailsImplementation> logger, IMapper mapper, ICachingService cachingService)
        {
            _restClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));

        }
        #endregion

        ///<inheritdoc/>
        public async Task<IEnumerable<BasicVehicleDetail>> GetBasicVehiclDetails(VehicleDetailsQuery vehicleDetailsQuery)
        {
            _logger.Log(LogLevel.Information, $"GetBasicVehiclDetails called with licenseplate: {vehicleDetailsQuery.Kenteken} and model: {vehicleDetailsQuery.Merk}");
            string key = $"{vehicleDetailsQuery.Kenteken}-{vehicleDetailsQuery.Merk}";
            var vehicleDetailsResult = await _cachingService.GetOrSetAsync(
                key, () => _restClient.GetRDWVehicleDetails(vehicleDetailsQuery.Kenteken, vehicleDetailsQuery.Merk));
            return _mapper.Map<IEnumerable<BasicVehicleDetail>>(vehicleDetailsResult);
        }
    }
}