using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Features.Reservations.Commands.CreateReservation;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using FluentAssertions;
using MediatR;
using Moq;
using Moq.EntityFrameworkCore;

namespace CoworkingManagement.UnitTests.Application.Reservations.Commands.CreateReservationTests;

public class CreateReservationCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IMediator> _mediator;
    private readonly CreateReservationCommandHandler _handler;

    public CreateReservationCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mediator = new Mock<IMediator>();
        _handler = new CreateReservationCommandHandler(_contextMock.Object, _currentUserServiceMock.Object, _cacheServiceMock.Object, _mediator.Object);
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
    public async Task Handle_Should_ReturnValidGuid_When_Reservation_Is_Created()
    {
        var command = GetValidCommand();

        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(new List<Reservation>());
        _currentUserServiceMock.Setup(x => x.UserId).Returns(Guid.NewGuid());

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeEmpty();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private CreateReservationCommand GetValidCommand()
    {
        return new CreateReservationCommand 
        (
            RoomId: Guid.NewGuid(),
            StartDate: DateTime.UtcNow.AddDays(1),
            EndDate: DateTime.UtcNow.AddDays(1).AddHours(2)
        );
    }
}