using CoworkingManagement.Application.Features.Reservations.Commands;
using CoworkingManagement.Application.Features.Reservations.Commands.UpdateReservation;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace CoworkingManagement.UnitTests.Application.Reservations.Commands.UpdateReservationTests;

public class UpdateReservationCommandValidatorTests
{
    private readonly UpdateReservationCommandValidator _validator;

    public UpdateReservationCommandValidatorTests()
    {
        _validator = new UpdateReservationCommandValidator();
    }

    [Fact]
    public void Validator_ShouldHaveError_When_EndDate_Id_Less_Than_Today()
    {
        var command = new UpdateReservationCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(-1));

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_ShouldHaveError_When_StartDate_Is_Less_Than_Today()
    {   
        var command = new UpdateReservationCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));

        var result = _validator.TestValidate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Have_Error_When_Required_Field_Is_Empty()
    {
        var reservatationId = Guid.Empty;

        var roomId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(1).AddHours(2);

        var command = new UpdateReservationCommand(reservatationId, roomId, startDate, endDate);

        var result = _validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(x => x.ReservationId);
    }
}