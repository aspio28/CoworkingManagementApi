using CoworkingManagement.Application.Features.Rooms.Commands.CreateRoomCommand;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace CoworkingManagement.UnitTests.Application.Rooms.Commands;

public class CreateRoomCommandValidatorTests
{
    private readonly CreateRoomCommandValidator _validator;

    public CreateRoomCommandValidatorTests()
    {
        _validator = new CreateRoomCommandValidator();
    }

    [Fact]
    public void Validator_ShouldHaveError_When_Capacity_Is_Negative()
    {
        var command = new CreateRoomCommand(-1, "Test Location");

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_ShouldHaveError_When_Location_Have_HTML_Tags()
    {   
        var command = new CreateRoomCommand(10, "<script>Test HTML tags</script>");

        var result = _validator.TestValidate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Have_Error_When_Location_Is_Empty()
    {
        var command = new CreateRoomCommand (10, "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Location);
    }
}