using System.Net;
using System.Net.Http.Headers;
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
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        var response = await _client.PostAsJsonAsync(
            "api/Reservations", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var reservation = await db.Reservations.SingleAsync();
        reservation.RoomId.Should().Be(request.roomId);
    }

    [Fact]
    public async Task Post_Should_ReturnFailure_When_Room_Is_Already_Occupied()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User("Test", "User", "conflict_already_occupied@test.com", "pass");
        var room = new Room(5, "Test Conflict");
        db.Users.Add(user);
        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        TestAuthHandler.CurrentUserId = user.Id;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        var start1 = DateTime.UtcNow.AddDays(1).Date.AddHours(10);
        var end1 = start1.AddHours(2);                          
        
        var start2 = start1.AddHours(1);                     
        var end2 = start2.AddHours(2);                          

        var command1 = new { RoomId = room.Id, StartDate = start1, EndDate = end1 };
        var response1 = await _client.PostAsJsonAsync("/api/Reservations", command1);
        response1.StatusCode.Should().Be(HttpStatusCode.Created);

        var command2 = new { RoomId = room.Id, StartDate = start2, EndDate = end2 };
        var response2 = await _client.PostAsJsonAsync("/api/Reservations", command2);

        response2.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
        
        var content = await response2.Content.ReadAsStringAsync();
        content.Should().Contain("Room is already booked for the selected time slot.");
    }

    [Fact]
    public async Task GetReservationList_Should_Return_Room_Details_When_Include_Is_Used()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User("Test", "User", "conflict_room_details@test.com", "pass");
        var room = new Room(5, "Test Conflict");


        db.Users.Add(user);
        db.Rooms.Add(room);

        await db.SaveChangesAsync();

        TestAuthHandler.CurrentUserId = user.Id;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
        
        var request = new
        {
            roomId = room.Id,
            startDate = DateTime.UtcNow.AddDays(1),
            endDate = DateTime.UtcNow.AddDays(2)
        };

        var responseCreate = await _client.PostAsJsonAsync("api/Reservations", request);
        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = responseCreate.Headers.Location?.ToString();
        var createdId = location?.Split('/').Last();

        var response = await _client.GetAsync($"/api/Reservations/{createdId}");
        var json = await response.Content.ReadAsStringAsync();

        json.Should().Contain("roomLocation");
        json.Should().Contain("roomCapacity"); 
    }
}
