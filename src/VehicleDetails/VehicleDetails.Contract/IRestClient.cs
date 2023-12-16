using VehicleDetails.DomainModel;

namespace VehicleDetails.Contract
{
    public interface IRestClient
    {
        /// <summary>
        /// Get the vehicle details from RDW API
        /// </summary>
        /// <param name="licensePlate">vehicle license plate.</param>
        /// <param name="brand">vehicle brand.</param>
        /// <returns></returns>
        Task<IEnumerable<RDWApiVehicleDataResponse>> GetRDWVehicleDetails(string? licensePlate, string? brand);
    }
}