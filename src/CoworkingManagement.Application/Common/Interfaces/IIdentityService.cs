namespace CoworkingManagement.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Result, string Token)> LoginAsync(string email, string password);
    Task<bool> ResgisterAsync(string email, string password, string firstName, string lastName);
}