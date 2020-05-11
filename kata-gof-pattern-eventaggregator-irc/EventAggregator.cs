using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("kata-gof-pattern-eventaggregator-irc-tests")]

namespace kata_gof_pattern_eventaggregator_irc
{
    public class EventAggregator : IEventAggregator
    {
        private readonly object _lock = new object();

        private readonly Dictionary<Type, List<WeakReference>> _subscribers =
            new Dictionary<Type, List<WeakReference>>();

        public void Subscribe(object subscriber)
        {
            var subscriberTypes = subscriber.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

            lock (_lock)
            {
                var weakReference = new WeakReference(subscriber);
                foreach (var subscriberType in subscriberTypes)
                {
                    var subscribers = GetSubscribers(subscriberType);
                    subscribers.Add(weakReference);
                }
            }
        }

        public void Publish<T>(T message)
        {
            var subscriberType = typeof(ISubscriber<>).MakeGenericType(typeof(T));
            List<WeakReference> subscribersForType;
            lock (_lock)
            {
                if (!_subscribers.ContainsKey(subscriberType)) return;

                subscribersForType = _subscribers[subscriberType];
            }

            var subscribersToRemove = new List<WeakReference>();
            foreach (var subscriber in subscribersForType)
                if (subscriber.IsAlive)
                {
                    var castSubscriber = (ISubscriber<T>) subscriber.Target;
                    castSubscriber.Consume(message);
                }
                else
                {
                    subscribersToRemove.Add(subscriber);
                }

            subscribersForType.RemoveAll(subscriber => subscribersToRemove.Contains(subscriber));
        }

        internal List<WeakReference> GetSubscribers(Type subscriberType)
        {
            List<WeakReference> subscribersForType;

            lock (_lock)
            {
                var found = _subscribers.TryGetValue(subscriberType, out subscribersForType);
                if (!found)
                {
                    subscribersForType = new List<WeakReference>();
                    _subscribers.Add(subscriberType, subscribersForType);
                }
            }

            return subscribersForType;
        }
    }
}