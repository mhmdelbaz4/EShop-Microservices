using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
            (ILogger<LoggingBehavior<TRequest,TResponse>> logger)
            : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[Start] Handling {RequestName} for {ResponseName} response. ", typeof(TRequest).Name, typeof(TResponse).Name);
        
        var timer = Stopwatch.StartNew();
        timer.Start();
        var response = await next();
        timer.Stop();
        
        var ellapsedMilliseconds = timer.ElapsedMilliseconds;
        if(ellapsedMilliseconds > 3000)
            logger.LogWarning("[Performance] Handling {RequestName} for {ResponseName} response took {ElapsedMilliseconds}ms which is longer than expected.", typeof(TRequest).Name, typeof(TResponse).Name, ellapsedMilliseconds);
        
        logger.LogInformation("[End] Handled {RequestName} in {ElapsedMilliseconds}ms for {ResponseName} response.", typeof(TRequest).Name, timer.ElapsedMilliseconds, typeof(TResponse).Name);
        return response;
    }
}
