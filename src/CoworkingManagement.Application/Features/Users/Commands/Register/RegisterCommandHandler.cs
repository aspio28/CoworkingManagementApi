using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Users.Commands.Register;

internal sealed class RegisterCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwt, ICacheService cache) : IRequestHandler<RegisterCommand, Unit>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    private readonly IJwtTokenGenerator _jwt = jwt ?? throw new ArgumentNullException(nameof(jwt));
    private readonly ICacheService _cache = cache ?? throw new ArgumentNullException(nameof(cache));

    public async Task<Unit> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var existsEmail = _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken: cancellationToken) ?? throw new DomainException("User already exists");

        var user = new User(
            name: command.Name,
            lastName: command.LastName,
            email: command.Email,
            password: _passwordHasher.Hash(command.Password)
        );

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        cache.Invalidate("Users");
        return Unit.Value;
    }
}