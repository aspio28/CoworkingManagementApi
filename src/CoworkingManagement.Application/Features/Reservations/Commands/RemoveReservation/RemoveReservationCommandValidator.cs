using FluentValidation;

namespace CoworkingManagement.Application.Features.Reservations.Commands.RemoveReservation;

public class RemoveReservationCommandValidator: AbstractValidator<RemoveReservationCommand>
{
    public RemoveReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId).NotEmpty().WithMessage("Reservation ID is required.");
    }
}