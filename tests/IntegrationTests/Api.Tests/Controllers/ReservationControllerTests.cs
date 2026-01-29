using System.Net;
using System.Net.Http.Json;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Infrastructure.Persistence;
using CoworkingManagement.IntegrationsTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoworkingManagement.IntegrationsTests.Controllers;

[Collection("Integration tests")]
public class ReservationsControllerTests
{
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ReservationsControllerTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Should_Create_Reservation()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        var user = new User (
            name: "User", 
            lastName: "Test",
            email: "user@test.com",
            password: "hashed_password"
        );
        db.Users.Add(user);

        var room = new Room(0, "Test Location");
        
        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        TestAuthHandler.CurrentUserId = user.Id;

        var request = new
        {
            roomId = room.Id,
            startDate = DateTime.UtcNow.AddDays(1),
            endDate = DateTime.UtcNow.AddDays(2)
        };

        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        var response = await _client.PostAsJsonAsync(
            "api/Reservations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var reservation = await db.Reservations.SingleAsync();
        reservation.RoomId.Should().Be(request.roomId);
    }
}
