using Mediator;

namespace Mediator;

public interface ICourier
{
    void Subscribe<TNotification>(Action<TNotification> handler)
       where TNotification : INotification;

    void Subscribe<TNotification>(Action<TNotification, CancellationToken> handler)
        where TNotification : INotification;

    void Subscribe<TNotification>(Func<TNotification, ValueTask> handler)
        where TNotification : INotification;

    void Subscribe<TNotification>(Func<TNotification, CancellationToken, ValueTask> handler)
        where TNotification : INotification;

    void UnSubscribe<TNotification>(Action<TNotification> handler)
        where TNotification : INotification;

    void UnSubscribe<TNotification>(Action<TNotification, CancellationToken> handler)
        where TNotification : INotification;

    void UnSubscribe<TNotification>(Func<TNotification, ValueTask> handler)
        where TNotification : INotification;

    void UnSubscribe<TNotification>(Func<TNotification, CancellationToken, ValueTask> handler)
        where TNotification : INotification;
}