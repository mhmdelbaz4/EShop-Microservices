
namespace Catalog.Api.Features.Products.GetProducts;

public record GetProductsQuery(string category,
                                int pageNumber= 1,
                                int pageSize = 10) : IQuery<GetProductsResponse>;

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResponse>
{
    public async Task<GetProductsResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = session.Query<Product>().AsQueryable();
        if(!string.IsNullOrWhiteSpace(request.category))
            query = query.Where(p => p.Categories.Contains(request.category));


        var products =await query.Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();

        return new GetProductsResponse(products);
    }
}
