namespace VehicleDetails.DomainModel.Options
{
    public class ApiAuthenticationOptions
    {
        public const string Name = "ApiAuth";
        public const string AuthKeyName = "XApiKey";

        public string ApiKey { get; set; }
    }
}
