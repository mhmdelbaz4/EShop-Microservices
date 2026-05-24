using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products.UpdateProduct;

public record UpdateProduct(Guid id,
                            string name,
                            string description,
                            string imageFile,
                            decimal price,
                            List<string> categories);


public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async ([FromBody]UpdateProduct dto,
                                        ISender sender) =>
        {
            try
            {
                UpdateProductCommand command = dto.Adapt<UpdateProductCommand>();
                var response = await sender.Send(command);
                return Results.Ok(response);
            }catch(NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }

        }).WithName("UpdateProduct")
          .Produces<Product>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Update Product")
          .WithDescription("Updates an existing product by its unique identifier.");
    }
}
