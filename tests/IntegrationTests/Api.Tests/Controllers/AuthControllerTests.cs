using System.Net;
using System.Net.Http.Json;
using CoworkingManagement.Application.Common.Models;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Infrastructure.Auth;
using CoworkingManagement.Infrastructure.Persistence;
using CoworkingManagement.IntegrationsTests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CoworkingManagement.IntegrationsTests.Controllers;

[Collection("Integration tests")]
public class AuthControllerTests
{
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly PasswordHasher _passwordHandler;

    public AuthControllerTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _passwordHandler = new PasswordHasher();
    }

    [Fact]
    public async Task Login_With_Valid_Credentials_Returns_Valid_Token()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var email = "user@logintest.com";
        var password = "userTestPass";

        var user = new User("User", "Test", email, _passwordHandler.Hash(password));

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var request = new
        {
            email,
            password
        };

        var response = await _client.PostAsJsonAsync(
            "api/Auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<AuthResult>();
        body!.Token.Should().NotBeNullOrEmpty();
    } 

    [Fact]
    public async Task Login_With_Invalid_Credentials_Returns_Unauthorized()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var email = "admin@coworking.com";
        var password = "admin1";

        var request = new
        {
            email,
            password
        };

        var response = await _client.PostAsJsonAsync(
            "api/Auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    } 
}