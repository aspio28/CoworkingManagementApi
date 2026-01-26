using CoworkingManagement.Domain.Enums;
using MediatR;

namespace CoworkingManagement.Application.Features.Users.Commands.UpdateUserRole;

public record UpdateUserRoleCommand (
    Guid Id,
    UserRole Role
) : IRequest<Unit>;