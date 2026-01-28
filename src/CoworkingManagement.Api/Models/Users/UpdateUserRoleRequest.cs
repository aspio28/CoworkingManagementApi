using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Api.Models.Rooms;

public record UpdateUserRoleRequest(
    UserRole Role
);