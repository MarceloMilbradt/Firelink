
using NWled.Models;
using System.Drawing;

namespace Firelink.Application.Common.Interfaces;

public interface IWledService
{
    Task<IEnumerable<string>> GetEffects(CancellationToken cancellationToken);
    Task<IEnumerable<string>> GetPalettes(CancellationToken cancellationToken);
    Task<State?> GetState(CancellationToken cancellationToken);
    Task SetState(State state, CancellationToken cancellationToken);
    Task<bool> TurnOn(CancellationToken cancellationToken);
}
