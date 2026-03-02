using PS.CacheCheatsheet_Lite.Caching.Abstractions;
using PS.CacheCheatsheet_Lite.Caching.Filters;
using PS.CacheCheatsheet_Lite.Caching.KeyBuilder;
using PS.CacheCheatsheet_Lite.Caching.Redis;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<ICacheKeyBuilder, HttpCacheKeyBuilder>();
builder.Services.AddScoped<CacheResourceFilter>();



builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
