using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository,CachedBasketRepository>();
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddMarten(options =>
{
    string BasketConnectionString = builder.Configuration.GetConnectionString("BasketDb") ?? throw new Exception("Connection string 'BasketDb' not found.");
    options.Connection(BasketConnectionString);
}).UseLightweightSessions();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Connection string 'Redis' not found.");
    options.InstanceName = "BasketApi";
});
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
                .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler(options => { });
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapCarter();
app.Run();