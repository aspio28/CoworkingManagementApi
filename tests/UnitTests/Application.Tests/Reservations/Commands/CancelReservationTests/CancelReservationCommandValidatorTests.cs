using CoworkingManagement.Application.Features.Reservations.Commands.CancelReservation;
using FluentValidation.TestHelper;

namespace CoworkingManagement.UnitTests.Application.Reservations.Commands.CancelReservationTests;

public class CancelReservationCommandValidatorTests
{
    private readonly CancelReservationCommandValidator _validator;

    public CancelReservationCommandValidatorTests()
    {
        _validator = new CancelReservationCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_ReservationId_Is_Empty()
    {
        var command = new CancelReservationCommand(Guid.Empty);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.ReservationId);
    }
}