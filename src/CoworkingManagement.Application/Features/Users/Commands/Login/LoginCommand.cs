using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Users.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
): IRequest<AuthResult>;