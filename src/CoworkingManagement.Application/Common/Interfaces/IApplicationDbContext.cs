using CoworkingManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CoworkingManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Room> Rooms { get; }
    DbSet<Reservation> Reservations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}