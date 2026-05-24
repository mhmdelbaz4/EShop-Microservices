using Carter;
using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});
builder.Services.AddMarten(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("CatalogDb")
                        ?? throw new Exception("Catalog Connection string not found!"); ;
    options.Connection(connectionString);
}).UseLightweightSessions();

var app = builder.Build();
app.MapCarter();
app.UseHttpsRedirection();

app.Run();
