using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
                : IPipelineBehavior<TRequest, TResponse>
                 where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(request)));
        var isValidState = results.All(r => r.IsValid);
        if (!isValidState)
        {
            var errors = results.SelectMany(r => r.Errors)
                                .Where(f => f != null)
                                .ToList();

            throw new ValidationException(errors);
        }
        return await next();
    }
}
