namespace Basket.Api.Features.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart ShoppingCart): ICommand<StoreBasketResult>;
public record StoreBasketResult(bool Success);

public class StoreBasketHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        var result  = await basketRepository.AddBasket(request.ShoppingCart);
        return new StoreBasketResult(result);
    }
}
