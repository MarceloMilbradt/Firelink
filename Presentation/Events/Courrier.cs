using Mediator;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Firelink.Presentation.Events;

public sealed class MediatorCourier : ICourier, INotificationHandler<INotification>
{
    private readonly ConcurrentDictionary<Type, ConcurrentBag<(Delegate action, bool needsToken)>> _actions = new();


    public async ValueTask Handle(INotification notification, CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();

        if (!_actions.TryGetValue(notificationType, out var subscribers)) subscribers = [];

        foreach (var (action, needsToken) in subscribers)
        {
            object[] parameters = needsToken ? [notification, cancellationToken] : [notification];

            var result = action.DynamicInvoke(parameters);
            if (result is ValueTask task) await task;
        }
    }

    public void Subscribe<TNotification>(Action<TNotification> handler)
        where TNotification : INotification =>
        Subscribe<TNotification>((handler, false));

    public void Subscribe<TNotification>(Action<TNotification, CancellationToken> handler)
        where TNotification : INotification =>
        Subscribe<TNotification>((handler, true));

    public void Subscribe<TNotification>(Func<TNotification, ValueTask> handler)
        where TNotification : INotification =>
        Subscribe<TNotification>((handler, false));

    public void Subscribe<TNotification>(Func<TNotification, CancellationToken, ValueTask> handler)
        where TNotification : INotification =>
        Subscribe<TNotification>((handler, true));

    
    public void UnSubscribe<TNotification>(Action<TNotification> handler)
        where TNotification : INotification =>
        UnSubscribe<TNotification>((Delegate)handler);

    public void UnSubscribe<TNotification>(Action<TNotification, CancellationToken> handler)
        where TNotification : INotification =>
        UnSubscribe<TNotification>((Delegate)handler);

    public void UnSubscribe<TNotification>(Func<TNotification, ValueTask> handler)
        where TNotification : INotification =>
        UnSubscribe<TNotification>((Delegate)handler);

    public void UnSubscribe<TNotification>(Func<TNotification, CancellationToken, ValueTask> handler)
        where TNotification : INotification =>
        UnSubscribe<TNotification>((Delegate)handler);

    private void Subscribe<TNotification>((Delegate handler, bool needsCancellation) subscriber)
        where TNotification : INotification
    {
        var notificationType = typeof(TNotification);


            if (_actions.TryGetValue(notificationType, out var subscribers))
            {
                subscribers.Add(subscriber);
            }
            else
            {
                _actions.TryAdd(notificationType, new ConcurrentBag<(Delegate, bool)>(new[] { subscriber }));
            }
        
    }

    private void UnSubscribe<TNotification>(Delegate handler)
        where TNotification : INotification
    {
        var notificationType = typeof(TNotification);
        Remove(handler, notificationType);
    }

    private void Remove(Delegate handler, Type notificationType)
    {
        if (!_actions.TryGetValue(notificationType, out var subscribers)) return;

        var remainingSubscribers = new ConcurrentBag<(Delegate, bool)>(subscribers.Where(subscriber => subscriber.action != handler));

        _actions.TryRemove(notificationType, out _);
        _actions.TryAdd(notificationType, remainingSubscribers);
    }

}