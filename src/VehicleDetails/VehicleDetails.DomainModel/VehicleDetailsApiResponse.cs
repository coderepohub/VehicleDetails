namespace VehicleDetails.DomainModel
{
    /// <summary>
    /// Crypto Quote Api response to return response which contains Status - Error or Success and the list of data.
    /// </summary>
    /// <typeparam name="T">Type of Data</typeparam>
    public class VehicleDetailsApiResponse<T>
    {
        /// <summary>
        /// Status of Api - Success or Error
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Basic Vehicle Details data.
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
