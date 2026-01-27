namespace CoworkingManagement.Application.Common.Interfaces;

public interface ICacheService
{
    CancellationTokenSource GetExpirationToken(string key);
    void Invalidate(string key);
}