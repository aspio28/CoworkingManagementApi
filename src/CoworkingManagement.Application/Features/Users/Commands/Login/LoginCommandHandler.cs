using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Users.Commands.Login;

internal sealed class LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwt) : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    private readonly IJwtTokenGenerator _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));

    public async Task<AuthResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email);

        if (user is null || !_passwordHasher.Verify(command.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = _jwt.GenerateToken(
            user.Id,
            user.Email,
            user.Role
        );

        return new AuthResult(
            token,
            DateTime.UtcNow.AddHours(8)
        );
    }
}