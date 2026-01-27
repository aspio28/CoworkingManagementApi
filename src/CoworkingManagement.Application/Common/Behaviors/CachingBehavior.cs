using CoworkingManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace CoworkingManagement.Application.Common.Behavious;

public class CachingBehavior<TRequest, TResponse>(IMemoryCache _cache, ILogger<CachingBehavior<TRequest, TResponse>> _logger, ICacheService _cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("{RequestName} is checking cache.", requestName);

        if (_cache.TryGetValue(request.CacheKey, out TResponse? response))
        {
            _logger.LogInformation("Cache hit for {RequestName} with key: {Key}", requestName, request.CacheKey);
            return response!;
        }

        _logger.LogInformation("Cache miss for {RequestName}. Fetching from database.", requestName);

        response = await next();

        var cacheOptions = new MemoryCacheEntryOptions()
            .AddExpirationToken(new CancellationChangeToken(_cacheService.GetExpirationToken(request.CacheTag).Token))
            .SetAbsoluteExpiration(request.Expiration ?? TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

        _cache.Set(request.CacheKey, response, cacheOptions);

        return response;
    }
}