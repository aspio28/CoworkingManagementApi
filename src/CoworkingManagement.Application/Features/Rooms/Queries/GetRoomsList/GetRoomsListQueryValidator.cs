using FluentValidation;

namespace CoworkingManagement.Application.Features.Rooms.Queries.GetRoomsList;

public class GetRoomsListQueryValidator : AbstractValidator<GetRoomsListQuery>
{
    public GetRoomsListQueryValidator()
    {
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("EndTime is required when StartDate is provided.")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("StartDate is required when EndTime is provided.")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Start time must be before end time.");
    }
}