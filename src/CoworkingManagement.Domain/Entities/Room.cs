using CoworkingManagement.Domain.Common;
using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Domain.Entities;

public class Room: BaseEntity
{
    public int Capacity { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public RoomStatus Status { get; private set; }
    public ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();

    public Room(int capacity, string location)
    {
        Id = Guid.NewGuid();
        Capacity = capacity;
        Status = RoomStatus.Available;
        Location = location;
    }
    public void Update(int? capacity = null, RoomStatus? status = null, string? location = null)
    {
        if (capacity.HasValue)
            Capacity = capacity.Value;

        if (status.HasValue)
            Status = status.Value;

        if (location != null) 
        {
            Location = location;
        }
    }
}