using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using kata_gof_pattern_eventaggregator_irc;
using Moq;
using Xunit;

namespace kata_gof_pattern_eventaggregator_irc_tests
{
    public class EventAggregatorLoadTest
    {
        public EventAggregatorLoadTest()
        {
            _messagesMock = new Mock<IMessageView>();
            _messageArgs = new List<string>();
            _messagesMock.Setup(x => x.Add(Capture.In(_messageArgs)));

            _eventAggregator = new EventAggregator();

            _messageService = new MessageAppService(_eventAggregator);

            _synchronizedStartEvent = new ManualResetEvent(false);
        }

        private const double SecondsBeforeFailing = 1.0;

        private readonly EventAggregator _eventAggregator;
        private readonly Mock<IMessageView> _messagesMock;
        private readonly List<string> _messageArgs;
        private readonly MessageAppService _messageService;
        private readonly ManualResetEvent _synchronizedStartEvent;

        private UserAppService CreateUserAppService(Mock<IMessageView> messagesMock)
        {
            _synchronizedStartEvent.WaitOne(TimeSpan.FromSeconds(SecondsBeforeFailing));

            var userService = new UserAppService(_eventAggregator, messagesMock.Object);
            return userService;
        }

        [Fact]
        public void
            When_10000_consumers_register_with_EventAggregator_from_diffeent_threads_THEN_All_consumers_receive_all_messages()
        {
            var numberOfUserServices = 10000;
            var tasks = new Task<UserAppService>[numberOfUserServices];
            for (var index = 0; index < numberOfUserServices; index++)
                tasks[index] = Task.Factory.StartNew(() => CreateUserAppService(_messagesMock));

            _synchronizedStartEvent.Set();
            Task.WaitAll(tasks);

            var userServices = tasks.Select(task => task.Result).ToList();

            _messageService.Send("Hello World", "bob", "alice");

            var expected = Enumerable.Range(0, numberOfUserServices).Select(x => "bob: Hello World").ToList();
            Assert.Equal(expected.Count, _messageArgs.Count);
            Assert.Equal(expected, _messageArgs);

            // Use the userService in order to prevent compiler from optimizing the userService away.
            userServices.ForEach(Assert.NotNull);
        }
    }
}