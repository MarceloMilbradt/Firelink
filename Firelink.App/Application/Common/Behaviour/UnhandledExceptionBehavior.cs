using Mediator;
using Microsoft.Extensions.Logging;

namespace Firelink.Application.Common.Behavior;

public sealed class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IMessage
{
    public ValueTask<TResponse> Handle(TRequest request,  CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        try
        {
            return next(request, cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogError(ex, "Firelink Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}