using CoworkingManagement.Application.Features.Reservations.Commands;
using CoworkingManagement.Application.Features.Reservations.Commands.CreateReservation;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace CoworkingManagement.UnitTests.Application.Reservations.Commands.CreateReservationTests;

public class CreateReservationCommandValidatorTests
{
    private readonly CreateReservationCommandValidator _validator;

    public CreateReservationCommandValidatorTests()
    {
        _validator = new CreateReservationCommandValidator();
    }

    [Fact]
    public void Validator_ShouldHaveError_When_StartDate_Is_Grather_Than_EndDate()
    {
        var command = new CreateReservationCommand(Guid.NewGuid(), DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(1));

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_ShouldHaveError_When_StartDate_Is_Less_Than_Today()
    {   
        var command = new CreateReservationCommand (Guid.NewGuid(), DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));

        var result = _validator.TestValidate(command);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("RoomId")]
    [InlineData("EndDate")]
    public void Should_Have_Error_When_Required_Field_Is_Empty(string fieldToInvalidate)
    {
        var roomId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(1);
        var endDate = DateTime.UtcNow.AddDays(1).AddHours(2);

        switch (fieldToInvalidate)
        {
            case "RoomId": roomId = Guid.Empty; break;
            case "EndDate": 
                endDate = default;
                break;
        }

        var command = new CreateReservationCommand(roomId, startDate, endDate);

        var result = _validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(fieldToInvalidate);
    }
}