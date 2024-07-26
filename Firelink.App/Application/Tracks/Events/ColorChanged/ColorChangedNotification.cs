using Firelink.App.Shared;
using Mediator;

namespace Firelink.Application.Tracks.Events.ColorChanged;

public sealed record ColorChangedNotification(Hsv NewColor) : INotification;