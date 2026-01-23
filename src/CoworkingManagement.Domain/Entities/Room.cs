using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    public int Capacity { get; set; }
    public RoomStatus Status { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}