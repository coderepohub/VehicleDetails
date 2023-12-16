using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel.Options;
using VehicleDetails.Implementation.Caching;

namespace VehicleDetails.UnitTests
{
    public class CachingServiceTests
    {

        private readonly IMemoryCache _memoryCache;
        private readonly Mock<IOptions<CacheOptions>> _mockOptions;
        private readonly CacheOptions cacheOptions;
        private readonly Mock<ILogger<CachingService>> _mockLogger;
        private readonly ICachingService _cachingService;

        public CachingServiceTests()
        {
            // Arrange
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockOptions = new Mock<IOptions<CacheOptions>>();
            cacheOptions = new CacheOptions();
            cacheOptions.DefaultBaseKey = "DefaultBaseKey";
            cacheOptions.ExpirationTimeInMinute = 10;
            _mockOptions.Setup(o => o.Value).Returns(cacheOptions);

            _mockLogger = new Mock<ILogger<CachingService>>();

            _cachingService = new CachingService(_memoryCache, _mockOptions.Object, _mockLogger.Object);

        }


        [Fact]
        public void CachingService_Constructor_ShouldThrowArgumentNullException_WhenMemoryCacheIsNull()
        {
            string param = "memoryCache";
            Action act = () => new CachingService(null, _mockOptions.Object, _mockLogger.Object);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null. (Parameter '{param}')")
                .And.ParamName.Should().Be(param);
        }

        [Fact]
        public void CachingService_Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            string param = "options";
            Action act = () => new CachingService(_memoryCache, null, _mockLogger.Object);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null. (Parameter '{param}')")
                .And.ParamName.Should().Be(param);
        }

        [Fact]
        public void CachingService_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            string param = "logger";
            Action act = () => new CachingService(_memoryCache, _mockOptions.Object, null);

            act.Should().Throw<ArgumentNullException>()
                .WithMessage($"Value cannot be null. (Parameter '{param}')")
                .And.ParamName.Should().Be(param);
        }

        [Fact]
        public async Task GetOrSetAsync_ShouldThrowArgumentException_WhenKeyIsNullOrEmpty()
        {
            string key = null;
            var getData = new Mock<Func<Task<int>>>();
            string errorLogMessage = "Key cannot be null or empty.";

            Func<Task> act = async () => await _cachingService.GetOrSetAsync(key, getData.Object);

            await act.Should().ThrowAsync<ArgumentException>();

            _mockLogger.Verify(x => x.Log(
      LogLevel.Error,
       It.IsAny<EventId>(),
       It.Is<It.IsAnyType>((v, t) => v.ToString() == errorLogMessage),
       It.IsAny<Exception>(),
       (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task GetOrSetAsync_ShouldThrowArgumentNullException_WhenGetDataIsNull()
        {
            string key = "someKey";
            Func<Task<int>> getData = null;
            string errorLogMessage = "getData cannot be null.";

            Func<Task> act = async () => await _cachingService.GetOrSetAsync(key, getData);

            await act.Should().ThrowAsync<ArgumentNullException>()
                 .WithMessage("Value cannot be null. (Parameter 'getData')");
            _mockLogger.Verify(x => x.Log(
      LogLevel.Error,
       It.IsAny<EventId>(),
       It.Is<It.IsAnyType>((v, t) => v.ToString() == errorLogMessage),
       It.IsAny<Exception>(),
       (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }


        [Fact]
        public async Task GetOrSetAsync_ShouldReturnCachedData_WhenDataExistsInCache()
        {
            // Arrange
            var key = "key";
            var cachekey = $"{cacheOptions.DefaultBaseKey}_{key}";
            var expectedData = "expectedData";
            _memoryCache.Set(cachekey, new Lazy<Task<string>>(() => Task.FromResult(expectedData)), new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));

            // Act
            var actualData = await _cachingService.GetOrSetAsync(key, async () => "fallbackData");

            // Assert
            actualData.Should().Be(expectedData);
        }

        [Fact]
        public async Task GetOrSetAsync_ShouldLoadAndCacheData_WhenDataDoesNotExistInCache()
        {
            // Arrange
            var key = "key";
            var cachekey = $"{cacheOptions.DefaultBaseKey}_{key}";
            var expectedData = "expectedData";

            var cachingService = new CachingService(_memoryCache, _mockOptions.Object, _mockLogger.Object);

            //var lazyData = new Lazy<Task<string>>(async () => expectedData);
            // Act
            var actualData = await cachingService.GetOrSetAsync(key, async () => expectedData);

            // Assert
            actualData.Should().Be(expectedData);

            // Check if data was cached
            var lazyData = _memoryCache.Get<Lazy<Task<string>>>(cachekey);
            lazyData.Should().NotBeNull();
            //(await lazyData.Value).Should().Be(expectedData);
            (await lazyData.Value).Should().Be(expectedData);
        }

        [Fact]
        public async Task GetOrSetAsync_ShouldUseFallbackData_WhenDataDoesNotExistInCacheAndLoadDataFails()
        {
            // Arrange
            var key = "key";
            var fallbackData = "fallbackData";

            // Act
            Func<Task> act = async () => await _cachingService.GetOrSetAsync<string>(key, async () => throw new Exception());

            await act.Should().ThrowAsync<Exception>();
        }
    }
}