using System.Text;

namespace PS.CacheCheatsheet_Lite.Caching.KeyBuilder
{
    public class HttpCacheKeyBuilder : ICacheKeyBuilder
    {
        private const string Prefix = "app";
        private const string Version = "v1";

        public string Build(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var method = request.Method;
            var path = request.Path.ToString().ToLowerInvariant();

            var normalizedQuery = NormalizeQuery(request.Query);

            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{Prefix}:{Version}:{method}:{path}");

            if (!string.IsNullOrEmpty(normalizedQuery))
                keyBuilder.Append($"?{normalizedQuery}");

            return keyBuilder.ToString();
        }

        private static string NormalizeQuery(IQueryCollection query)
        {
            if (!query.Any())
                return string.Empty;

            var ordered = query
                .OrderBy(q => q.Key)
                .SelectMany(q => q.Value.Select(v => $"{q.Key.ToLowerInvariant()}={v}"));

            return string.Join("&", ordered);
        }
    }
}