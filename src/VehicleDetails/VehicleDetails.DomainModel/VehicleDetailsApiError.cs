namespace VehicleDetails.DomainModel
{
    /// <summary>
    /// Display error at Vehicle Details API level 
    /// </summary>
    public class VehicleDetailsApiError
    {
        /// <summary>
        /// Error Messsage
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error Details
        /// </summary>
        public string Details { get; set; }
    }
}
