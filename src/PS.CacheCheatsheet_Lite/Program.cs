using PS.CacheCheatsheet_Lite.Caching.KeyBuilder;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddScoped<ICacheKeyBuilder, HttpCacheKeyBuilder>();



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
