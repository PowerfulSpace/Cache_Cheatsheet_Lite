namespace PS.CacheCheatsheet_Lite.Caching.KeyBuilder
{
    public interface ICacheKeyBuilder
    {
        string Build(HttpContext httpContext);
    }
}
