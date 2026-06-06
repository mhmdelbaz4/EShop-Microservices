namespace Basket.Api.Models;
public class ShoppingCart
{
    [Identity]
    public string UserName { get; set; } =string.Empty;
    public List<ShoppingCartItem> CartItems { get; set; } = new();
    public decimal TotalPrice { get => CartItems.Sum(i => i.Price); }
}
