using Microsoft.Extensions.Caching.Distributed;

namespace JoBoard.AuthService.Application.Contracts;

public interface ICustomRedisClient
{
    T Get<T>(string key) where T : class;
    void Set<T>(string key, T value) where T : class;
    void Set<T>(string key, T value, DistributedCacheEntryOptions options) where T : class;
    void Remove(string key);
}