using System.Text.Json;

namespace Basket.Api.Data;

public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache)
            : IBasketRepository
{
    public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var basket =await cache.GetStringAsync(userName);
        if(!string.IsNullOrEmpty(basket))
            return JsonSerializer.Deserialize<ShoppingCart>(basket);

        var basketFromDb = await basketRepository.GetBasket(userName, cancellationToken);
        if(basketFromDb != null)
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basketFromDb));
        
        return basketFromDb;
    }
    public async Task<bool> AddBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        var result = await basketRepository.AddBasket(shoppingCart, cancellationToken);
        if(result)
            await cache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart));
        
        return result;
    }

    public Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        var result = basketRepository.DeleteBasket(userName, cancellationToken);
        if (result.Result)
            cache.RemoveAsync(userName);
        return result;
    }

}
