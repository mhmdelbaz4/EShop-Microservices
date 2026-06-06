namespace Basket.Api.Features.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName): ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool Success);

public class DeleteBasketHandler(IBasketRepository basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        bool result = await basketRepository.DeleteBasket(request.UserName, cancellationToken);
        return new DeleteBasketResult(result);
    }
}
