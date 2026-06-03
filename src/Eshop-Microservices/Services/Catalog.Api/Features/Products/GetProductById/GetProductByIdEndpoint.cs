using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products.GetProductById;


public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async ([FromRoute]Guid id, ISender sender) =>
        {
            var response = await sender.Send(new GetProductByIdQuery(id));
            return Results.Ok(response);

        }).WithName("GetProductById")
          .Produces<Product>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Get Product by ID")
          .WithDescription("Retrieves a product by its unique identifier.");
    }
}
