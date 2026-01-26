using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Users.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string Name,
    string LastName
): IRequest<Unit>;