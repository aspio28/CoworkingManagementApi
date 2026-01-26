using CoworkingManagement.Application.Common.Interfaces;

namespace CoworkingManagement.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string password, string passwordHash) => 
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
}