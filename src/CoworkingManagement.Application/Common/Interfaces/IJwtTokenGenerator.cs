using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(
        Guid UserId,
        string email,
        UserRole role
    );
}