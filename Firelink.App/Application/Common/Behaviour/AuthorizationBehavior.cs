using System.Reflection;
using Firelink.Application.Common.Interfaces;
using Firelink.Application.Common.Security;
using Mediator;

namespace Firelink.Application.Common.Behavior;

public sealed class AuthorizationBehavior<TRequest, TResponse>(ISpotifyApi spotifyApi) : IPipelineBehavior<TRequest, TResponse> where TRequest : IMessage
{
    public ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (!authorizeAttributes.Any()) return next(request, cancellationToken);
        
        if (!spotifyApi.IsUserLoggedIn())
        {
            throw new UnauthorizedAccessException();
        }

        return next(request, cancellationToken);
    }
}