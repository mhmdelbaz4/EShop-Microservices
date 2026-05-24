using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Products.CreateProduct;

public record CreateProductDto(string name,
                                string description,
                                string imageFile,
                                decimal price,
                                List<string> categories);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async ([FromBody] CreateProductDto dto, ISender sender) =>
        {
            CreateProductCommand command = dto.Adapt<CreateProductCommand>();
            var createProductResponse = await sender.Send(command);
            return Results.Created($"/products/{createProductResponse.id}", createProductResponse);
        }).WithName("Create Product")
        .WithDescription("Create Product")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);
        
    }
}
