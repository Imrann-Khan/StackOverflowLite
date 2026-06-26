using System.Text.Json;
using StackExchange.Redis;
using StackOverflowLite.Application.Services.Interfaces;

namespace StackOverflowLite.Infrastructure.Caching;

public class RedisCacheService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if(value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(value.ToString()!);
    }

    public async Task<long> IncrementAsync(string key) => await _db.StringIncrementAsync(key);

    public async Task RemoveAsync(string key) => await _db.KeyDeleteAsync(key);

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        if (expiry.HasValue)
            await _db.StringSetAsync(key, json, expiry.Value);
        else
            await _db.StringSetAsync(key, json);
    }
}