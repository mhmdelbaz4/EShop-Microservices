using BuildingBlocks.Exceptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

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

var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler(options => { });
app.MapCarter();
app.Run();