namespace CourseLibrary.Caching;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Hybrid;

public class HybridCacheProvider : ICacheProvider
{
    private readonly HybridCache _hybridCache;

    public HybridCacheProvider(HybridCache hybridCache)
    {
        _hybridCache = hybridCache;
    }

    public async Task<T> GetOrSetAsync<T>(
      string key,
      Func<CancellationToken, ValueTask<T>> factory,
      HybridCacheEntryOptions? options = null,
      CancellationToken cancellationToken = default)
    {
        return await _hybridCache.GetOrCreateAsync(
            key,
            factory,
            options,
            cancellationToken: cancellationToken);
    }


    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _hybridCache.RemoveAsync(key, cancellationToken);
    }
}

