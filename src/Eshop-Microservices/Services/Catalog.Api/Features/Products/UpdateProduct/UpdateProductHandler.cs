using BuildingBlocks.Exceptions;

namespace Catalog.Api.Features.Products.UpdateProduct;

public record UpdateProductCommand( Guid Id,
                                    string Name,
                                    string Description,
                                    string ImageFile,
                                    decimal Price,
                                    List<string> Categories) : ICommand<UpdateProductResponse>;

public record UpdateProductResponse(bool isSuccessful);

public class UpdateProductHandler(IDocumentSession session) 
            : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await session.LoadAsync<Product>(request.Id, cancellationToken);
        if(product ==null)
            throw new NotFoundException("Product not found");

        product = request.Adapt<Product>();
        session.Update<Product>(product);

        await session.SaveChangesAsync(cancellationToken);
        return new UpdateProductResponse(false);
    }

}
