using Firelink.App.Shared;
using MediatR;

namespace Firelink.Application.Tracks.Events.ColorChanged;

public record ColorChangedNotification(Hsv NewColor) : INotification;