using Firelink.Application.Common.Interfaces;
using Mediator;
using OneOf.Types;

namespace Firelink.Application.CustomEffects.Commands.DeleteCustomEffect;

public record DeleteCustomEffectCommand(string Id) : IRequest<Success>;


public sealed class DeleteCustomEffectCommandHandler : IRequestHandler<DeleteCustomEffectCommand, Success>
{
    private readonly IWledCustomEffectProvider _wledCustomEffectProvider;

    public DeleteCustomEffectCommandHandler(IWledCustomEffectProvider wledCustomEffectProvider)
    {
        _wledCustomEffectProvider = wledCustomEffectProvider;
    }

    public async ValueTask<Success> Handle(DeleteCustomEffectCommand request, CancellationToken cancellationToken)
    {
        await _wledCustomEffectProvider.RemoveCustomEffect(request.Id, cancellationToken);
        return new Success();
    }
}
