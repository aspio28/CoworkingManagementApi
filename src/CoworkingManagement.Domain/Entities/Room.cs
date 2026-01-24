using CoworkingManagement.Domain.Common;
using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Domain.Entities;

public class Room: BaseEntity
{
    public int Capacity { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public RoomStatus Status { get; private set; }
    public ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();

    public Room(int capacity, RoomStatus status, string location)
    {
        Id = Guid.NewGuid();
        Capacity = capacity;
        Status = status;
        Location = location;
    }
    public void Update(int capacity, RoomStatus status, string location)
    {
        Capacity = capacity;
        Status = status;
        Location = location;
    }
}