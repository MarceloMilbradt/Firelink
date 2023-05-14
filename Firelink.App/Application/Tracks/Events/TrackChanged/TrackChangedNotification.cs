using Firelink.App.Shared;
using MediatR;

namespace Firelink.Application.Tracks.Events.TrackChanged;

public record TrackChangedNotification(TrackDto newTrack) : INotification;

