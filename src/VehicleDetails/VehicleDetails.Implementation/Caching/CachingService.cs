using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel;
using VehicleDetails.DomainModel.Options;

namespace VehicleDetails.Implementation.Caching
{
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheOptions _cacheOptions;
        private readonly int _expirationTime;
        private readonly ILogger<CachingService> _logger;
        public CachingService(IMemoryCache memoryCache, IOptions<CacheOptions> options, ILogger<CachingService> logger)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _cacheOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (!int.TryParse(_cacheOptions.ExpirationTimeInMinute.ToString(), out _expirationTime))
            {
                _logger.LogWarning("Invalid expiration time specified in cache options. Defaulting to 10 minutes.");
                _expirationTime = 10;
            }
        }

        ///<inheritdoc/>
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getData)
        {
            if (string.IsNullOrEmpty(key))
            {
                _logger.Log(LogLevel.Error, "Key cannot be null or empty.");
                throw new ArgumentException(nameof(key));
            }
            if (getData is null)
            {
                _logger.Log(LogLevel.Error, $"getData cannot be null.");
                throw new ArgumentNullException(nameof(getData));
            }

            string cachingKey = $"{_cacheOptions.DefaultBaseKey}_{key}";

            T cachedData;
            // Trying to get data from the cache.
            if (!_memoryCache.TryGetValue(cachingKey, out Lazy<Task<T>> lazyData))
            {
                // If the cache is empty, create a new Lazy object to load the data from the getData function
                lazyData = new Lazy<Task<T>>(async () => await getData());

                // Set the cache options
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(_expirationTime)); // Set the expiration time


                try
                {
                    cachedData = await lazyData?.Value;
                    if (cachedData is not null)
                    {
                        _memoryCache.Set(cachingKey, lazyData, cacheOptions);
                    }
                }
                catch(HttpResponseException hex)
                {
                    throw new HttpResponseException(hex.StatusCode,hex.Value);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message, ex);
                    throw;
                }
            }
            else
            {
                cachedData = await lazyData?.Value;
            }
            return cachedData;
        }
    }
}
