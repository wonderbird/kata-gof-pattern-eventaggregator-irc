using System;
using System.Collections.Generic;
using System.Linq;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class EventAggregator
    {
        private readonly Dictionary<Type, List<WeakReference>> _subscribers = new Dictionary<Type, List<WeakReference>>();

        private readonly object _lock = new object();

        public void Publish<T>(T message)
        {
            // TODO: Adapt Publish to the implementation in the example

            var subscriberType = typeof(ISubscriber<>).MakeGenericType(typeof(T));
            List<WeakReference> subscribersForType;
            lock (_lock)
            {
                if (!_subscribers.ContainsKey(subscriberType))
                {
                    return;
                }

                subscribersForType = _subscribers[subscriberType];
            }

            foreach (var subscriber in subscribersForType)
            {
                if (subscriber.IsAlive)
                {
                    var castSubscriber = (ISubscriber<T>) subscriber.Target;
                    castSubscriber.Consume(message);
                }
            }
        }

        public void Subscribe(object subscriber)
        {
            var subscriberTypes = subscriber.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

            lock (_lock)
            {
                foreach (var subscriberType in subscriberTypes)
                {
                    List<WeakReference> subscribersForType;

                    if (!_subscribers.TryGetValue(subscriberType, out subscribersForType))
                    {
                        subscribersForType = new List<WeakReference>();
                        _subscribers.Add(subscriberType, subscribersForType);
                    }

                    subscribersForType.Add(new WeakReference(subscriber));
                }
            }
        }
    }
}