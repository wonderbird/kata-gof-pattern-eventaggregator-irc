using System;
using System.Collections.Generic;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class EventAggregator
    {
        private readonly Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();

        public void Publish<T>(T message)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                return;
            }

            var subscribersForType = _subscribers[type];
            foreach (var subscriber in subscribersForType)
            {
                var castSubscriber = (ISubscriber<T>) subscriber;
                castSubscriber.Consume(message);
            }
        }

        public void Subscribe<T>(ISubscriber<T> subscriber)
        {
            var type = typeof(T);
            var subscriberList = new List<object>();
            subscriberList.Add(subscriber);
            _subscribers.Add(type, subscriberList);
        }
    }
}