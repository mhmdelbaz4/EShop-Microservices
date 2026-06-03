using HealthChecks.NpgSql;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehaviors([typeof(LoggingBehavior<,>)]);
    config.AddOpenBehaviors([typeof(ValidationBehavior<,>)]);
});

builder.Services.AddMarten(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("CatalogDb")
                        ?? throw new Exception("Catalog Connection string not found!"); ;
    options.Connection(connectionString);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<InitialData>();

builder.Services.AddHealthChecks()
                    .AddNpgSql(builder.Configuration.GetConnectionString("CatalogDb")!);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapCarter();
app.Run();
