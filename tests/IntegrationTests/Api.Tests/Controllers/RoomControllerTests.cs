using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using CoworkingManagement.Infrastructure.Persistence;
using CoworkingManagement.IntegrationsTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoworkingManagement.IntegrationsTests.Controllers;

[Collection("Integration tests")]
public class RoomControllerTests
{
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RoomControllerTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Delete_Should_Perform_SoftDelete_When_Room_Has_No_Reservations()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User("Admin Test", "User", $"room@test.com", "pass");
        user.UpdateRole(UserRole.Admin);

        var room = new Room(10, "Remove Room");
        db.Users.Add(user);
        db.Rooms.Add(room);
        await db.SaveChangesAsync();

        TestAuthHandler.CurrentUserId = user.Id;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        var response = await _client.DeleteAsync($"/api/Rooms/{room.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        db.ChangeTracker.Clear();

        var deletedRoom = await db.Rooms
            .IgnoreQueryFilters() 
            .FirstOrDefaultAsync(r => r.Id == room.Id);

        deletedRoom.Should().NotBeNull();
        deletedRoom.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Should_Return_Conflict_When_Room_Has_Active_Reservations()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User("Admin Test", "User", "conflict_room_active@test.com", "pass");
        user.UpdateRole(UserRole.Admin);

        var room = new Room(5, "Test Occupied Room");

        db.Users.Add(user);
        db.Rooms.Add(room);
        
        var start = DateTime.UtcNow.AddDays(1);
        var end = start.AddHours(2);
        var reservation = new Reservation(user.Id, room.Id, ReservationStatus.Reserved, start, end);
        
        db.Reservations.Add(reservation);
        await db.SaveChangesAsync();

        TestAuthHandler.CurrentUserId = user.Id;
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);

        var response = await _client.DeleteAsync($"/api/Rooms/{room.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("upcoming reservations");

        var roomInDb = await db.Rooms
            .IgnoreQueryFilters() 
            .FirstOrDefaultAsync(r => r.Id == room.Id);

        roomInDb.Should().NotBeNull();
        roomInDb.IsDeleted.Should().BeFalse();
    }
}