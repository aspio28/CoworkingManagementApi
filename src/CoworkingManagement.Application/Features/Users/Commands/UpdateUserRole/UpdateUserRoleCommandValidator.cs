using FluentValidation;

namespace CoworkingManagement.Application.Features.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommandValidtor : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidtor()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required");
    }
}