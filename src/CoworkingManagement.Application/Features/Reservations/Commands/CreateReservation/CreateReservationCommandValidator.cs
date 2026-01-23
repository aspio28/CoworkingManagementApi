using CoworkingManagement.Application.Features.Reservations.Commands.CreateReservation;
using FluentValidation;

namespace CoworkingManagement.Application.Features.Reservations.Commands;

public class CreateReservationCommandValidator: AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Room ID is required.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Reservation date cannot be in the past.");
        RuleFor(x => x.StartTime).LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");
    }
}