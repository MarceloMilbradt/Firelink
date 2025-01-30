using Mediator;
using Microsoft.Extensions.Logging;

namespace Firelink.Application.Common.Behaviour;

public sealed class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : INotification
{
    private readonly ILogger _logger = logger;

    public ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Firelink Notification: {Name} {@Request}",
            requestName, message);

        return next(message, cancellationToken);

    }
}