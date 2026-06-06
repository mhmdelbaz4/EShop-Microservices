namespace Basket.Api.Features.Basket.DeleteBasket;

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("basket/{UserName}", async (string UserName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(UserName));
            return Results.Ok(result);
        }).WithName("Delete Basket")
          .WithDescription("Delete basket by user name")
          .Produces<DeleteBasketResult>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
