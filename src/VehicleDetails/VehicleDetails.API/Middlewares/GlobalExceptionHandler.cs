using Newtonsoft.Json;
using System.Net;
using VehicleDetails.DomainModel.Enums;
using VehicleDetails.DomainModel;

namespace VehicleDetails.API.Middlewares
{
    /// <summary>
    /// Global Exception handler which handles entire API level exceptions
    /// </summary>
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpResponseException httpEx)
            {
                _logger.Log(LogLevel.Error, httpEx.Message, httpEx);
                await HandleExceptionAsync(context, httpEx, httpEx.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, ex);
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, int httpStatusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpStatusCode;
            string customMessage = (ex as HttpResponseException)?.Value != null ? ((HttpResponseException)ex)?.Value as string : string.Empty;

            var response = new VehicleDetailsApiResponse<VehicleDetailsApiError>
            {
                Status = HttpStatus.Error.ToString(),
                Data = new List<VehicleDetailsApiError>
                  {
                      new VehicleDetailsApiError
                      {
                          Message = "An error occurred while processing your request.",
                          Details = string.IsNullOrEmpty(customMessage)? ex.Message : customMessage
                      }
                  }
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
