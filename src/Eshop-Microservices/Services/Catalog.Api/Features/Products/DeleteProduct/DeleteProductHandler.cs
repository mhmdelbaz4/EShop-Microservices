using BuildingBlocks.Exceptions;

namespace Catalog.Api.Features.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResponse>;

public record DeleteProductResponse(bool isSuccess);

public class DeleteProductHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResponse>
{
    public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await session.LoadAsync<Product>(request.Id);
        if (product is null)
            throw new NotFoundException("Product not found!");

        session.Delete<Product>(request.Id);
        await session.SaveChangesAsync();
        return new DeleteProductResponse(true);
    }
}
