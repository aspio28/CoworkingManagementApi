using FluentValidation;

namespace CoworkingManagement.Application.Features.Rooms.Commands.RemoveRoomCommand;

public class RemoveRoomCommandValidator : AbstractValidator<RemoveRoomCommand>
{
    public RemoveRoomCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("RoomId is required.");
    }
}