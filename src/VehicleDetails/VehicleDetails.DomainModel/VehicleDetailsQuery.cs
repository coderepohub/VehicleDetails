namespace VehicleDetails.DomainModel
{
    /// <summary>
    /// Vehicle Details Query, query to fetch vehicle details data.
    /// </summary>
    public class VehicleDetailsQuery
    {
        /// <summary>
        /// License plate number.
        /// </summary>
        public string? Kenteken { get; set; }

        /// <summary>
        /// Vehicle Model.
        /// </summary>
        public string? Merk { get; set; }
    }
}
