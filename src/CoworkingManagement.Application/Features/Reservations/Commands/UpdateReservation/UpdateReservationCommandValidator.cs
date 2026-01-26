using CoworkingManagement.Application.Features.Reservations.Commands.UpdateReservation;
using FluentValidation;

namespace CoworkingManagement.Application.Features.Reservations.Commands;

public class UpdateReservationCommandValidator: AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId).NotEmpty().WithMessage("Reservation ID is required.");
        RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Reservation start date cannot be in the past.");
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Reservation end date cannot be in the past.");
    }
}