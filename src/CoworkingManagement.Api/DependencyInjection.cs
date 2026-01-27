using System.Text.Json.Serialization;
using CoworkingManagement.Application.Common.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoworkingManagement;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });;
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Coworking API", Version = "v1"});

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authrization header using Bearer scheme. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                    },
                    Array.Empty<string>()
                }
            });

            c.OperationFilter<IgnoreCachePropertiesFilter>();
        });

        return services;
    }

    public class IgnoreCachePropertiesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var cacheProps = typeof(ICacheableQuery).GetProperties().Select(p => p.Name);
            
            // Eliminamos los parámetros de la operación en Swagger que coincidan con la interfaz
            var parametersToRemove = operation.Parameters
                .Where(p => cacheProps.Any(cp => string.Equals(cp, p.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            foreach (var parameter in parametersToRemove)
            {
                operation.Parameters.Remove(parameter);
            }
        }
    }
}