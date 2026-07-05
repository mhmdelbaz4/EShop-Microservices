using Discount.gRPC.Data;
using Discount.gRPC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DiscountDb")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
    dbContext.Database.Migrate();
}

app.MapGrpcService<DiscountService>();

app.Run();
