using Firelink.Domain;
using Mediator;

namespace Firelink.Application.Tracks.Events.TrackChanged;

public sealed record TrackChangedNotification(TrackDto Track) : INotification;
