using CoworkingManagement.Application.Common.Extensions;
using FluentValidation;

namespace CoworkingManagement.Application.Features.Users.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .NotContainHtml("Password");
        RuleFor(x => x. Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.")
            .NotContainHtml("Name");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.")
            .NotContainHtml("LastName");
    }
}