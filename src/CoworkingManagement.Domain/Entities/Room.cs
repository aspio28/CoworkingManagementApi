using CoworkingManagement.Domain.Common;

namespace CoworkingManagement.Domain.Entities;

public class Room: BaseEntity
{
    public int Capacity { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public ICollection<Reservation> Reservations { get; private set; } = [];
    public bool IsDeleted { get; private set; } = false;

    public Room(int capacity, string location)
    {
        Id = Guid.NewGuid();
        Capacity = capacity;
        Location = location;
    }
    public void Update(int? capacity = null, string? location = null)
    {
        if (capacity.HasValue)
            Capacity = capacity.Value;

        if (location != null) 
        {
            Location = location;
        }
    }
    public void Delete()
    {
        IsDeleted = true;
    }
}