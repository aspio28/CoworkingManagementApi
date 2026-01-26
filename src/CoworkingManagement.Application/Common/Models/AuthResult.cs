namespace CoworkingManagement.Application.Common.Models;

public record AuthResult(
    string Token,
    DateTime ExpiredAt
);