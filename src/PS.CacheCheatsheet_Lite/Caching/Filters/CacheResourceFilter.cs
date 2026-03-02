using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PS.CacheCheatsheet_Lite.Caching.Abstractions;
using PS.CacheCheatsheet_Lite.Caching.KeyBuilder;

namespace PS.CacheCheatsheet_Lite.Caching.Filters
{
    public class CacheResourceFilter : IAsyncResourceFilter
    {
        private readonly ICacheService _cache;
        private readonly ICacheKeyBuilder _keyBuilder;
        private readonly ILogger<CacheResourceFilter> _logger;
        private readonly int _durationMinutes;
        private readonly bool _useSlidingExpiration;

        public CacheResourceFilter(
            ICacheService cache,
            ICacheKeyBuilder keyBuilder,
            ILogger<CacheResourceFilter> logger,
            int durationMinutes,
            bool useSlidingExpiration)
        {
            _cache = cache;
            _keyBuilder = keyBuilder;
            _logger = logger;
            _durationMinutes = durationMinutes;
            _useSlidingExpiration = useSlidingExpiration;
        }

        public async Task OnResourceExecutionAsync(
            ResourceExecutingContext context,
            ResourceExecutionDelegate next)
        {
            var cacheKey = _keyBuilder.Build(context.HttpContext);

            var cached = await _cache.GetAsync<object>(cacheKey, context.HttpContext.RequestAborted);

            if (cached is not null)
            {
                _logger.LogInformation("Cache HIT: {Key}", cacheKey);
                context.Result = new OkObjectResult(cached);
                return;
            }

            _logger.LogInformation("Cache MISS: {Key}", cacheKey);

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okResult)
            {
                TimeSpan? absolute = _useSlidingExpiration ? null : TimeSpan.FromMinutes(_durationMinutes);
                TimeSpan? sliding = _useSlidingExpiration ? TimeSpan.FromMinutes(_durationMinutes) : null;

                await _cache.SetAsync(
                    cacheKey,
                    okResult.Value,
                    absoluteExpiration: absolute,
                    slidingExpiration: sliding,
                    cancellationToken: context.HttpContext.RequestAborted);

                _logger.LogInformation("Cache SET: {Key}", cacheKey);
            }
        }
    }
}