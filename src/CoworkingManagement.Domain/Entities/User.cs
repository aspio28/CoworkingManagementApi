using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}