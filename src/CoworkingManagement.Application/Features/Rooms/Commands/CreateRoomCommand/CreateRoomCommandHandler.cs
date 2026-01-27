using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using MediatR;

namespace CoworkingManagement.Application.Features.Rooms.Commands.CreateRoomCommand;

internal sealed class CreateRoomCommandHandler(IApplicationDbContext context, ICacheService cache) : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ICacheService _cache = cache ?? throw new ArgumentNullException(nameof(cache));


    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room(request.Capacity, request.Location);

        await _context.Rooms.AddAsync(room, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _cache.Invalidate("Rooms");
        return room.Id;
    }
}