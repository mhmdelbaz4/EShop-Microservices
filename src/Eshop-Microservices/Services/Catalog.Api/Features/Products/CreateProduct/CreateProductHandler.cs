namespace Catalog.Api.Features.Products.CreateProduct;
public record CreateProductCommand(string Name,
                                  string Description,
                                  string ImageFile,
                                  decimal Price,
                                  List<string> Categories) : ICommand<CreateProductResponse>;

public record CreateProductResponse(Guid id);

public class CreateProductHandler(IDocumentSession session) 
            : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = request.Adapt<Product>();
        session.Store(product);
        await session.SaveChangesAsync();
        return new CreateProductResponse(product.Id);
    }
}
