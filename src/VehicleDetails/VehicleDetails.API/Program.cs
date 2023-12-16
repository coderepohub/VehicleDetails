
using AutoMapper;
using Microsoft.OpenApi.Models;
using VehicleDetails.API.Middlewares;
using VehicleDetails.Contract;
using VehicleDetails.DomainModel.Options;
using VehicleDetails.HttpConnector;
using VehicleDetails.HttpConnector.Helpers;
using VehicleDetails.Implementation;
using VehicleDetails.Implementation.Caching;
using VehicleDetails.Implementation.Mappers;

namespace VehicleDetails.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vehicle Details API", Version = "v1" });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API Key Authentication",
                    In = ParameterLocation.Header,
                    Name = ApiAuthenticationOptions.AuthKeyName,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
            });


            builder.Services.AddOptions<RDWApiOption>().Bind(builder.Configuration.GetSection(RDWApiOption.Name));
            builder.Services.AddOptions<CacheOptions>().Bind(builder.Configuration.GetSection(CacheOptions.Name));
            builder.Services.AddOptions<ApiAuthenticationOptions>().Bind(builder.Configuration.GetSection(ApiAuthenticationOptions.Name));

            builder.Services.AddVehicleDetailsHttpClient();
            builder.Services.AddScoped<IVehicleDetailsImplementation, VehicleDetailsImplementation>();
            builder.Services.AddScoped<IRestClient, RestClient>();
            builder.Services.AddScoped<IHttpClientProvider, HttpClientProvider>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BasicVehicleDetailsMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            //Memory Cache
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<ICachingService, CachingService>();

            var app = builder.Build();

            //Add Authorization Middleware
            app.UseMiddleware<AuthMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}