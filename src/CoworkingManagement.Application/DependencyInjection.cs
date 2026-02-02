using System.Reflection;
using CoworkingManagement.Application.Common.Behaviors;
using CoworkingManagement.Application.Common.Behavious;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CoworkingManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
    
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}