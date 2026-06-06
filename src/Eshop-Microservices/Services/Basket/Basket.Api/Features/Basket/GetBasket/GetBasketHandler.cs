using BuildingBlocks.Exceptions;

namespace Basket.Api.Features.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart ShoppingCart);


public class GetBasketHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var cart = await basketRepository.GetBasket(request.UserName, cancellationToken);
        if (cart == null)
            throw new NotFoundException("Basket not found.");

        return new GetBasketResult(cart);
    }
}
