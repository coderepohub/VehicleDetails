namespace VehicleDetails.DomainModel
{
    /// <summary>
    /// Basic Vehicle Details Information
    /// </summary>
    public class BasicVehicleDetail
    {
        /// <summary>
        /// License Plate of the Vehicle
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// Vehicle Brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Vehicle Model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Year of Vehicle Manufacturing
        /// </summary>
        public DateTime YearOfManufacture { get; set; }
    }
}
