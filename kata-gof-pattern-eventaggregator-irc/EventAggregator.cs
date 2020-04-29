using System;
using System.Collections.Generic;
using System.Linq;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class EventAggregator
    {
        private readonly Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();

        public void Publish<T>(T message)
        {
            // TODO: Adapt Publish to the implementation in the example

            var subscriberType = typeof(ISubscriber<>).MakeGenericType(typeof(T));
            if (!_subscribers.ContainsKey(subscriberType))
            {
                return;
            }

            var subscribersForType = _subscribers[subscriberType];
            foreach (var subscriber in subscribersForType)
            {
                var castSubscriber = (ISubscriber<T>) subscriber;
                castSubscriber.Consume(message);
            }
        }

        public void Subscribe(object subscriber)
        {
            // TODO: Create a requirement enforcing thread safety as in the example

            var subscriberTypes = subscriber.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

            foreach (var subscriberType in subscriberTypes)
            {
                List<object> subscribersForType;

                if (!_subscribers.TryGetValue(subscriberType, out subscribersForType))
                {
                    subscribersForType = new List<object>();
                    _subscribers.Add(subscriberType, subscribersForType);
                }

                subscribersForType.Add(subscriber);
            }
        }
    }
}