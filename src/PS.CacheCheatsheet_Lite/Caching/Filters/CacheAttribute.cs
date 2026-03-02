using Microsoft.AspNetCore.Mvc.Filters;

namespace PS.CacheCheatsheet_Lite.Caching.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAttribute : Attribute, IFilterFactory
    {
        public int DurationMinutes { get; }
        public bool UseSlidingExpiration { get; }

        public CacheAttribute(int durationMinutes, bool useSlidingExpiration = false)
        {
            DurationMinutes = durationMinutes;
            UseSlidingExpiration = useSlidingExpiration;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return ActivatorUtilities.CreateInstance<CacheResourceFilter>(
                serviceProvider,
                DurationMinutes,
                UseSlidingExpiration);
        }

        public bool IsReusable => false;
    }
}
