using Microsoft.AspNetCore.Mvc;
using System.Net;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel;
using VehicleDetails.DomainModel.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VehicleDetails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleDetailsController : ControllerBase
    {
        private readonly IVehicleDetailsImplementation _vehicleDetailsImplementation;
        private readonly ILogger<VehicleDetailsController> _logger;
        #region Constructor
        public VehicleDetailsController(IVehicleDetailsImplementation vehicleDetailsImplementation, ILogger<VehicleDetailsController> logger)
        {
            _vehicleDetailsImplementation = vehicleDetailsImplementation ?? throw new ArgumentNullException(nameof(vehicleDetailsImplementation));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        /// <summary>
        /// Get basic Vehicle Details.
        /// </summary>
        /// <param name="vehicleDetailsQuery">Query to fetch vehicle information, Kenteken or Merk</param>
        /// <returns>List of Vehicle Information matching the requested query.</returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPost(Name = "Get Vehicle Details.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDetailsApiResponse<BasicVehicleDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] VehicleDetailsQuery vehicleDetailsQuery)
        {
            _logger.Log(LogLevel.Information, $"Get Vehicle Details api endpoint called.");
            if (vehicleDetailsQuery == null)
            {
                _logger.Log(LogLevel.Error, $"Vehicle Details query should have some data");
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, $"{nameof(vehicleDetailsQuery)} should not be empty.");
            }
            var basicVehicleDetails = await _vehicleDetailsImplementation.GetBasicVehiclDetails(vehicleDetailsQuery);
            return Ok(new VehicleDetailsApiResponse<BasicVehicleDetail>
            {
                Status = HttpStatus.Success.ToString(),
                Data = basicVehicleDetails
            });
        }

    }
}
