using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Users.Commands.GetUserList;

internal sealed class GetRoomsListHandler : IRequestHandler<GetUserListQuery, PaginatedList<UserDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoomsListHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PaginatedList<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserDto { 
                Id = u.Id, 
                Name = u.Name, 
                LastName = u. LastName, 
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt, 
                CreatedBy = u.CreatedBy, 
                LastModifiedAt = u.LastModifiedAt, 
                LastModifiedBy = u.LastModifiedBy
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<UserDto>(items, count, request.PageNumber, request.PageSize);
    }
}