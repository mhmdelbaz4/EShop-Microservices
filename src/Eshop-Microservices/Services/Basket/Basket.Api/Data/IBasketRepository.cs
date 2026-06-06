namespace Basket.Api.Data;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellationToken = default);
    Task<bool> AddBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);

}
