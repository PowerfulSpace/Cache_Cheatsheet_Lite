using Microsoft.Extensions.Caching.Distributed;
using PS.CacheCheatsheet_Lite.Caching.Abstractions;
using System.Text.Json;

namespace PS.CacheCheatsheet_Lite.Caching.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            var cachedValue = await _cache.GetStringAsync(key, ct);

            if (cachedValue is null)
                return default;

            return JsonSerializer.Deserialize<T>(cachedValue, _serializerOptions);
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? absoluteExpiration = null,
            TimeSpan? slidingExpiration = null,
            CancellationToken ct = default)
        {
            var options = new DistributedCacheEntryOptions();

            if (absoluteExpiration.HasValue)
                options.AbsoluteExpirationRelativeToNow = absoluteExpiration;

            if (slidingExpiration.HasValue)
                options.SlidingExpiration = slidingExpiration;

            var serialized = JsonSerializer.Serialize(value, _serializerOptions);

            await _cache.SetStringAsync(key, serialized, options, ct);
        }

        public async Task RemoveAsync(string key, CancellationToken ct = default)
        {
            await _cache.RemoveAsync(key, ct);
        }

    }
}
