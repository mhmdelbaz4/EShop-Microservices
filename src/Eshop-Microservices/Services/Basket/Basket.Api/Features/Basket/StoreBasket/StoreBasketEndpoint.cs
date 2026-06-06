using Mapster;

namespace Basket.Api.Features.Basket.StoreBasket;

public record StoreProductDto(ShoppingCart ShoppingCart);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("basket", async ([AsParameters] StoreProductDto dto, ISender sender) =>
        {
            var command = dto.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result);
        }).WithName("Store Basket")
          .WithDescription("Store basket for user")
          .Produces<StoreBasketResult>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
