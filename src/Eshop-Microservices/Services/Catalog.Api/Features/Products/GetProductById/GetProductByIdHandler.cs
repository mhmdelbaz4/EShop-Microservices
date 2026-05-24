using BuildingBlocks.Exceptions;

namespace Catalog.Api.Features.Products.GetProductById;

public record GetProductByIdQuery(Guid id) : IQuery<GetProductByIdResponse>;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product? product = await session.LoadAsync<Product>(request.id, cancellationToken);

        if (product == null)
            throw new NotFoundException("Product not found");
        
        return new GetProductByIdResponse(product);
    }
}
