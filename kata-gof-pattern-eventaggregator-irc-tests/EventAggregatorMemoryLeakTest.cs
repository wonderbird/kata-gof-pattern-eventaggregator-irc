using System;
using kata_gof_pattern_eventaggregator_irc;
using Xunit;

namespace kata_gof_pattern_eventaggregator_irc_tests
{
    public class DisposableSubscriber : ISubscriber<string>, IDisposable
    {
        public void Dispose()
        {
        }

        public void Consume(string message)
        {
            throw new Exception("DisposableSubscriber.Consume should never be called.");
        }
    }

    public class EventAggregatorMemoryLeakTest
    {
        public EventAggregatorMemoryLeakTest()
        {
            _eventAggregator = new EventAggregator();
        }

        private readonly EventAggregator _eventAggregator;

        private void SubscribeAndDisposeSubscriber()
        {
            // see StackOverflow: "How can I write a unit test to determine whether an object can be garbage collected?"
            // https://stackoverflow.com/a/579001
            new Action(() =>
            {
                var subscriberStub = new DisposableSubscriber();
                _eventAggregator.Subscribe(subscriberStub);
            })();
        }

        [Fact]
        public void EventAggregatorPublish_SubscriberIsDisposed_RemovesSubscriber()
        {
            SubscribeAndDisposeSubscriber();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            _eventAggregator.Publish("Hello World");
            var subscribers = _eventAggregator.GetSubscribers(typeof(ISubscriber<string>));

            Assert.Empty(subscribers);
        }
    }
}