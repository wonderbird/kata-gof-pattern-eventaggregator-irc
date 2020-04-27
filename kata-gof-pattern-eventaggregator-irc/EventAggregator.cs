using System.Collections.Generic;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class EventAggregator
    {
        private readonly List<ISubscriber<LoginMessage>> _subscribers = new List<ISubscriber<LoginMessage>>();

        public void Publish(LoginMessage message)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.Consume(message);
            }
        }

        public void Subscribe(ISubscriber<LoginMessage> subscriber)
        {
            _subscribers.Add(subscriber);
        }
    }
}