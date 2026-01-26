using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoworkingManagement.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser(
    ApplicationDbContext _context, IPasswordHasher 
    _passwordHasher, 
    IConfiguration _configuration,
    ILogger<ApplicationDbContextInitialiser> _logger
    )
{
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
    private async Task TrySeedAsync()
    {
        var adminEmail = _configuration["DefaultAdmin:Email"];
        var adminPassword = _configuration["DefaultAdmin:Password"];

        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
        {
            _logger.LogWarning("DefaultAdmin configuration is missing in appsettings.json. Skipping seed.");
            return;
        }

        if(!_context.Users.Any(u => u.Email == adminEmail))
        {
            var admin = new User
            (
                _configuration["DefaultAdmin:Name"] ?? "Admin",
                _configuration["DefaultAdmin:LastName"] ?? "System",
                adminEmail,
                _passwordHasher.Hash(adminPassword)
            );

            admin.UpdateRole(UserRole.Admin);

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();
        }
    }
}