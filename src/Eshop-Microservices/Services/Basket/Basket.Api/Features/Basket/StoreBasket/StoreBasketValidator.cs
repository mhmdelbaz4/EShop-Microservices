namespace Basket.Api.Features.Basket.StoreBasket;
public class StoreBasketValidator: AbstractValidator<StoreBasketCommand>
{
    public StoreBasketValidator()
    {
        RuleFor(x => x.ShoppingCart)
            .NotNull().WithMessage("Shopping cart is required");

        RuleFor(x => x.ShoppingCart.UserName)
            .NotEmpty().WithMessage("User name is required");
    }
}
