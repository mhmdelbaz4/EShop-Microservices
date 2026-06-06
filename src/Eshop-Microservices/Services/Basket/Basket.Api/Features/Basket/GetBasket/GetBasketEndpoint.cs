namespace Basket.Api.Features.Basket.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("basket/{UserName}", async (string UserName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(UserName));
            return Results.Ok(result);
        }).WithName("Get Basket")
          .WithDescription("Get basket by user name")
          .Produces<GetBasketResult>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
