using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handlers;
using Carter;
using FluentValidation;
using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehaviors([typeof(ValidationBehavior<,>)]);
});

builder.Services.AddMarten(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("CatalogDb")
                        ?? throw new Exception("Catalog Connection string not found!"); ;
    options.Connection(connectionString);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseExceptionHandler(options => { });
app.MapCarter();
app.Run();
