using CoworkingManagement.Domain.Common;
using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Domain.Entities;

public class User: BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public ICollection<Reservation> Reservations { get; private set; } = [];

    public User(string name, string lastName, string email, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Email = email;
        Password = password;
        Role = UserRole.Member;
    }

    public void Update(string name, string lastName, string email, UserRole role)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Role = role;
    }

    public void UpdateRole(UserRole role)
    {
        Role = role;
    }
}