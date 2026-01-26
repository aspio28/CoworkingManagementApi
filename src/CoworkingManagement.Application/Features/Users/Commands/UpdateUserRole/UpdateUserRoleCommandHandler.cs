using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Users.Commands.UpdateUserRole;

internal sealed class UpdateUserRoleCommandHandler(IApplicationDbContext _context) : IRequestHandler<UpdateUserRoleCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(user), request.Id);
        }
        
        user.UpdateRole(request.Role);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}