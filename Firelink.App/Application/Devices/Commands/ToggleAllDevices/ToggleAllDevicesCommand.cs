using Firelink.Application.Common.Interfaces;
using Firelink.Application.Common.Result;
using Mediator;
using OneOf;
using OneOf.Types;

namespace Firelink.Application.Devices.Commands.ToggleAllDevices;

public record ToggleAllDevicesCommand(bool Power) : IRequest<OneOf<Success, WledHttpError>>
{
    public static readonly ToggleAllDevicesCommand Default = new(true);
}

public sealed class ToggleAllDevicesCommandHandler : IRequestHandler<ToggleAllDevicesCommand, OneOf<Success, WledHttpError>>
{
    private readonly IWledService _wledService;

    public ToggleAllDevicesCommandHandler(IWledService wledService)
    {
        _wledService = wledService;
    }

    public async ValueTask<OneOf<Success, WledHttpError>> Handle(ToggleAllDevicesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _wledService.TurnOn(cancellationToken);
            return new Success();
        }
        catch (HttpRequestException ex)
        {
            return new WledHttpError
            {
                code = ((int)ex.StatusCode.GetValueOrDefault()),
                message = ex.Message,
            };
        }
    }
}