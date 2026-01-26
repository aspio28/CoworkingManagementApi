using CoworkingManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoworkingManagement.Infrastructure.Extensions;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.SeedAsync();
    }
}