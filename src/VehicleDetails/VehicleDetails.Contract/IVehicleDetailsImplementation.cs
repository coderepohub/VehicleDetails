using VehicleDetails.DomainModel;

namespace VehicleDetails.Contract
{
    public interface IVehicleDetailsImplementation
    {
        /// <summary>
        /// Get basic vehicle details.
        /// </summary>
        /// <param name="vehicleDetailsQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<BasicVehicleDetail>> GetBasicVehiclDetails(VehicleDetailsQuery vehicleDetailsQuery);
    }
}
