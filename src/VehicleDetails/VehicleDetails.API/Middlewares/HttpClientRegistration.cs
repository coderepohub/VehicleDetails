using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using VehicleDetails.DomainModel.Options;

namespace VehicleDetails.API.Middlewares
{
    /// <summary>
    /// Adding Http Client Registration.
    /// </summary>
    public static class HttpClientRegistration
    {
        public static IServiceCollection AddVehicleDetailsHttpClient(this IServiceCollection services)
        {

            services.AddHttpClient(HttpClientSetting.ClientName, (serviceProvider, httpClient) =>
            {
                var rdwApiOptions = serviceProvider.GetRequiredService<IOptions<RDWApiOption>>().Value;
                _ = httpClient.BaseAddress = new Uri(rdwApiOptions.BaseUrl);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }


        /// <summary>
        /// Retry policy which will retry the operation up to 4 times with an increasing delay between retries.
        /// </summary>
        /// <returns></returns>
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// Apply Circuit Breaker Policy where it opens the circuit after 3 consecutive errors
        /// then wait for 30 seconds before allowing further attempts.
        /// </summary>
        /// <returns></returns>
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }
    }
}
