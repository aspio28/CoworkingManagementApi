namespace CoworkingManagement.Application.Common.Interfaces;

public interface ICacheableQuery
{
    string CacheKey { get; }
    string CacheTag { get; }
    TimeSpan? Expiration { get; }
}