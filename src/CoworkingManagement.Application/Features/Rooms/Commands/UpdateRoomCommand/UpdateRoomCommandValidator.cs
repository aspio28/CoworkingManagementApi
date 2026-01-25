using FluentValidation;

namespace CoworkingManagement.Application.Features.Rooms.Commands.UpdateRoomCommand;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("RoomId is required.");
        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be greater than zero.");
        When(x => x.Location != null, () =>
        {
            RuleFor(x => x.Location).NotEmpty().WithMessage("Location cannot be empty.");
        });
    }
}