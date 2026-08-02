namespace CourseLibrary.Caching;

using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface ICacheProvider
{
    /// <summary>
    /// Tries to get a cached item. If not found, executes the factory function and stores the result in cache.
    /// </summary>
    Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        HybridCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cache entry by its key.
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}

