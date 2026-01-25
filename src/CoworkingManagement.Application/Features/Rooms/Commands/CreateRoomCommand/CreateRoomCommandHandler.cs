using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using MediatR;

namespace CoworkingManagement.Application.Features.Rooms.Commands.CreateRoomCommand;

internal sealed class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateRoomCommandHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room(request.Capacity, request.Location);

        await _context.Rooms.AddAsync(room, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return room.Id;
    }
}