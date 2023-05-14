using MediatR;

namespace Firelink.Application.Tracks.Events.ColorChanged;

internal sealed class ColorChangedNotificationHandler : INotificationHandler<ColorChangedNotification>
{
    public Task Handle(ColorChangedNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            var color = notification.NewColor;
            if (string.IsNullOrEmpty(color))
            {
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessages.ERROR_WHILE_CONNECTING_DEVICES, ex);
        }
    }
}