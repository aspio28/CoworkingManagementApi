using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Application.Common.Models;

public record UserDto : BaseEntityDto
{
    public string Name { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public UserRole Role { get; init; }
}