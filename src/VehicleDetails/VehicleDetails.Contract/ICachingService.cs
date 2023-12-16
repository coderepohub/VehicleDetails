namespace VehicleDetails.Contract
{
    public interface ICachingService
    {
        /// <summary>
        /// Get or Set Data from Cache
        /// </summary>
        /// <typeparam name="T"> Generic return type</typeparam>
        /// <param name="key">cahce key</param>
        /// <param name="getData">call back function to call if data is not present in cahce.</param>
        /// <param name="expirationTime">expiration time for caching.</param>
        /// <returns></returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getData);
    }
}
