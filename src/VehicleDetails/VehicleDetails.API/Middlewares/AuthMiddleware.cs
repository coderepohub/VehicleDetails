using Microsoft.Extensions.Options;
using System.Net;
using VehicleDetails.DomainModel;
using VehicleDetails.DomainModel.Options;

namespace VehicleDetails.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKeyName;
        private readonly string _apiKey;

        public AuthMiddleware(RequestDelegate next, IOptions<ApiAuthenticationOptions> appSettings)
        {
            _next = next;
            _apiKey = appSettings.Value.ApiKey;
            _apiKeyName = ApiAuthenticationOptions.AuthKeyName;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isSwaggerRequest = context.Request.Path.StartsWithSegments("/swagger");

            if (!isSwaggerRequest)
            {
                if (!context.Request.Headers.TryGetValue(_apiKeyName, out var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Api Key was not provided");
                    return;
                }

                if (!_apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized client");
                    return;
                }
            }

            await _next(context);
        }
    }
}
