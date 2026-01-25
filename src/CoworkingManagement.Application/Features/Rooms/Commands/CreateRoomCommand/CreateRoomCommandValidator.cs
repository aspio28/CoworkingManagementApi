using System.Data;
using FluentValidation;

namespace CoworkingManagement.Application.Features.Rooms.Commands.CreateRoomCommand;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than zero.");
        RuleFor(x => x.Capacity).NotEmpty().WithMessage("Capacity is required.");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required.");
    }
}