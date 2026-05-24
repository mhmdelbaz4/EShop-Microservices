namespace Catalog.Api.Features.Products.GetProducts;

public record GetProductsDto(string category="",
                             int pageNumber = 1,
                             int pageSize = 10);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsDto dto, ISender sender) =>
        {
            GetProductsQuery query = dto.Adapt<GetProductsQuery>();
            var getProductsResponse = await sender.Send(query);
            return Results.Ok(getProductsResponse);
        });
    }
}
