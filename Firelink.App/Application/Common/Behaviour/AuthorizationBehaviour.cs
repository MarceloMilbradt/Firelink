using System.Reflection;
using Firelink.Application.Common.Interfaces;
using Firelink.Application.Common.Security;
using MediatR;

namespace Firelink.Application.Common.Behaviour;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ISpotifyApi _spotifyApi;

    public AuthorizationBehaviour(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (!authorizeAttributes.Any()) return await next();
        
        if (!_spotifyApi.IsUserLoggedIn())
        {
            throw new UnauthorizedAccessException();
        }
        return await next();
    }
}