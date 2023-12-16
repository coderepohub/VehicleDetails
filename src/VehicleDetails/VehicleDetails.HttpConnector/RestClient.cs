using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel;
using VehicleDetails.DomainModel.Enums;
using VehicleDetails.DomainModel.Options;

namespace VehicleDetails.HttpConnector
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly RDWApiOption _rdwApiOptions;
        private readonly ILogger<RestClient> _logger;

        #region Constructor
        public RestClient(IHttpClientProvider httpClientProvider, IOptions<RDWApiOption> options, ILogger<RestClient> logger)
        {
            _rdwApiOptions = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClientProvider = httpClientProvider ?? throw new ArgumentNullException(nameof(httpClientProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        ///<inheritdoc/>
        public async Task<IEnumerable<RDWApiVehicleDataResponse>> GetRDWVehicleDetails(string? licensePlate, string? brand)
        {
            if (string.IsNullOrWhiteSpace(licensePlate) && string.IsNullOrWhiteSpace(brand))
            {
                _logger.Log(LogLevel.Error, $"licenseplate and brand name are empty.");
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, $"Request should contain atleast one of these - kenteken or merk");
            }
            var uriBuilder = new StringBuilder();
            var hasQuery = false;
            if (!string.IsNullOrWhiteSpace(licensePlate))
            {
                uriBuilder.Append($"?kenteken={licensePlate}");
                hasQuery = true;
            }
            if (!string.IsNullOrWhiteSpace(brand))
            {
                uriBuilder.Append(hasQuery ? "&" : "?");
                uriBuilder.Append($"merk={brand}");
            }
            var uri = uriBuilder.ToString();
            _logger.Log(LogLevel.Information, $"Calling api with uri - {uri}.");
            var httpResponseMessage = await SendRequest(Method.GET, uri);
            if (httpResponseMessage is not null && httpResponseMessage.StatusCode is HttpStatusCode.OK)
            {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(httpResponseContent))
                {
                    throw new HttpResponseException((int)HttpStatusCode.BadGateway, $"API returned with no response");
                }

                var response = JsonConvert.DeserializeObject<IEnumerable<RDWApiVehicleDataResponse>>(httpResponseContent)!;
                if (response.Any())
                    return response;
                throw new HttpResponseException((int)HttpStatusCode.NotFound, $"No Vehicle Information was found with entered details.");
            }

            throw await HandleFailedresponse(httpResponseMessage);
        }

        #region Private Methods
        private static async Task<HttpResponseException> HandleFailedresponse(HttpResponseMessage? httpResponseMessage)
        {
            HttpStatusCode httpStatusCode = httpResponseMessage?.StatusCode == HttpStatusCode.BadRequest ? HttpStatusCode.BadRequest : HttpStatusCode.ServiceUnavailable;
            string responseMessage = httpResponseMessage?.Content is null ? string.Empty : await httpResponseMessage?.Content?.ReadAsStringAsync();
            return new HttpResponseException((int)httpStatusCode, responseMessage);
        }
        private async Task<HttpResponseMessage> SendRequest(Method method, string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException($"uri can not be null or empty");
            }

            HttpResponseMessage response = null;

            try
            {
                switch (method)
                {
                    case Method.GET:
                        response = await _httpClientProvider.GetJsonAsync(uri);
                        break;
                    default:
                        response = new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.MethodNotAllowed,
                        };
                        break;
                }
            }
            catch (Exception ex)
            {

                response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                };
            }
            return response;

        }

        #endregion
    }
}