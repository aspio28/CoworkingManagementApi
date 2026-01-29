using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Features.Reservations.Commands.CancelReservation;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using CoworkingManagement.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace CoworkingManagement.UnitTests.Application.Reservations.Commands.CancelReservationTests;

public class CancelReservationCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly CancelReservationCommandHandler _handler;

    public CancelReservationCommandHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _cacheServiceMock = new Mock<ICacheService>();

        _handler = new CancelReservationCommandHandler(_contextMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ThrowNotFoundException_When_Reservation_Does_Not_Exist()
    {
        var command = new CancelReservationCommand(Guid.NewGuid());
        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(new List<Reservation>());

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData(-10, -5, "Cannot remove past reservations.")]       
    [InlineData(-1, 2, "Cannot remove ongoing or past reservations.")]  
    public async Task Handle_Should_ThrowBusinessException_When_Reservation_Is_Past_Or_Ongoing(
        int hoursFromNowStart, int hoursFromNowEnd, string expectedMessage)
    {
        var reservation = new Reservation(
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            ReservationStatus.Reserved, 
            DateTime.UtcNow.AddHours(hoursFromNowStart), 
            DateTime.UtcNow.AddHours(hoursFromNowEnd));

        var reservationId = reservation.Id;
        var command = new CancelReservationCommand(reservationId);
        
        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(new List<Reservation> { reservation });

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BusinessException>()
                 .WithMessage(expectedMessage);
    }

    [Fact]
    public async Task Handle_Should_CancelReservation_When_Valid()
    {
        var reservation = new Reservation(
            Guid.NewGuid(), 
            Guid.NewGuid(), 
            ReservationStatus.Reserved, 
            DateTime.UtcNow.AddDays(1), 
            DateTime.UtcNow.AddDays(1).AddHours(1));

        var command = new CancelReservationCommand(reservation.Id);

        _contextMock.Setup(x => x.Reservations).ReturnsDbSet(new List<Reservation> { reservation });

        await _handler.Handle(command, CancellationToken.None);

        reservation.Status.Should().Be(ReservationStatus.Cancelled);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(x => x.Invalidate("Rooms"), Times.Once);
        _cacheServiceMock.Verify(x => x.Invalidate("Reservations"), Times.Once);
    }
}