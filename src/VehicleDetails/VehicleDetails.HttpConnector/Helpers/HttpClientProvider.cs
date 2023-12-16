using System.Net.Http;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel.Options;

namespace VehicleDetails.HttpConnector.Helpers
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly HttpClient _httpClient;
        public HttpClientProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientSetting.ClientName);
        }

        ///<inheritdoc/>
        public async Task<HttpResponseMessage> GetJsonAsync(string uri)
        {
            return await _httpClient.GetAsync(uri);
        }
    }
}
