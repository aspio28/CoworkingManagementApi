using System.Collections.Concurrent;
using CoworkingManagement.Application.Common.Interfaces;

namespace CoworkingManagement.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _tokens = new();

    public CancellationTokenSource GetExpirationToken(string tag)
    {
        return _tokens.GetOrAdd(tag, _ => new CancellationTokenSource());
    }

    public void Invalidate(string tag)
    {
        if (_tokens.TryRemove(tag, out var source))
        {
            source.Cancel();
            source.Dispose();
        }
    }
}