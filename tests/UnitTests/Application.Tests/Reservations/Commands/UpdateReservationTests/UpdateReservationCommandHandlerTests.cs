using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Features.Reservations.Commands.UpdateReservation;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace CoworkingManagement.UnitTests.Application.Reservations.Commands.CreateReservationTests;

public class UpdateReservationCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly UpdateReservationCommandHandler _handler;

    public UpdateReservationCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _cacheServiceMock = new Mock<ICacheService>();

        _handler = new UpdateReservationCommandHandler(_contextMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Failure_When_Room_Is_Already_Reserved()
    {
        var command = GetValidCommand();

        var roomId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingReservations = new List<Reservation> 
        { 
            new(
                userId,     
                roomId,                      
                ReservationStatus.Reserved,      
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(2)       
            ) 
        };
        
        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(existingReservations);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Handle_Should_UpdateReservation_When_Valid()
    {
        var newRoomId = Guid.NewGuid();

        var reservation = new Reservation(
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            ReservationStatus.Reserved, 
            DateTime.UtcNow.AddDays(1), 
            DateTime.UtcNow.AddDays(1).AddHours(1));

        var command = new UpdateReservationCommand(reservation.Id, newRoomId, null, null);

        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(new List<Reservation> { reservation });

        await _handler.Handle(command, CancellationToken.None);

        reservation.RoomId.Should().Be(newRoomId);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(x => x.Invalidate("Rooms"), Times.Once);
        _cacheServiceMock.Verify(x => x.Invalidate("Reservations"), Times.Once);
    }

    private UpdateReservationCommand GetValidCommand()
    {
        return new UpdateReservationCommand 
        (
            ReservationId: Guid.NewGuid(),
            RoomId: Guid.NewGuid(),
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(1).AddHours(2)
        );
    }
}