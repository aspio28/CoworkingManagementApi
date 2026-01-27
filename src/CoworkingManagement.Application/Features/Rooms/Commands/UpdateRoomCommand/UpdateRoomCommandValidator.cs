using CoworkingManagement.Application.Common.Extensions;
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
        RuleFor(x => x.Location).NotContainHtml("Location");
    }
}