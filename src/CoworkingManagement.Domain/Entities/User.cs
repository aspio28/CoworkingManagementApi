using CoworkingManagement.Domain.Common;
using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Domain.Entities;

public class User: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();

    public User(string name, string lastName, string email, UserRole role)
    {
        Id = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Email = email;
        Role = role;
    }

    public void Update(string name, string lastName, string email, UserRole role)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Role = role;
    }
}