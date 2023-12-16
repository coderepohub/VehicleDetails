namespace VehicleDetails.DomainModel.Options
{
    /// <summary>
    /// Cache configuration Optins
    /// </summary>
    public class CacheOptions
    {
        public const string Name = "CacheConfig";

        public string DefaultBaseKey { get; set; }
        public int ExpirationTimeInMinute { get; set; }

    }
}
