using BuildingBlocks.Exceptions;

namespace Catalog.Api.Features.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            try
            {
                var response = await sender.Send(new DeleteProductCommand(id));
                return Results.Ok(response);

            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }).WithName("Delete Product")
          .WithDescription("Delete Product")
          .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
