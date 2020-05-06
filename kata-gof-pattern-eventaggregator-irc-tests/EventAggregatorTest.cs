using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using kata_gof_pattern_eventaggregator_irc;
using Moq;
using Xunit;

namespace kata_gof_pattern_eventaggregator_irc_tests
{
    public class EventAggregatorTest
    {
        private readonly DateTime _timestamp;
        private readonly string _timestampString;
        private readonly string _username;
        private readonly EventAggregator _eventAggregator;
        private readonly AuthenticationAppService _authService;
        private readonly Mock<IMessageView> _messagesMock;
        private readonly MessageAppService _messageService;

        public EventAggregatorTest()
        {
            _timestamp = DateTime.Now;
            _timestampString = _timestamp.ToString(Settings.TimeStampFormat);

            _username = "username";
            _messagesMock = new Mock<IMessageView>();
            _eventAggregator = new EventAggregator();

            _authService = new AuthenticationAppService(_eventAggregator);
            _messageService = new MessageAppService(_eventAggregator);
        }

        [Fact]
        public void BillingView_UserLogsIn_ShowsLoginTimestamp()
        {
            var billingService = new BillingAppService(_eventAggregator, _messagesMock.Object);
            _authService.Login(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"{_username} logged in at {_timestampString}"));
        }

        [Fact]
        public void BillingView_UserLogsOut_ShowsLogoutTimestamp()
        {
            var billingService = new BillingAppService(_eventAggregator, _messagesMock.Object);
            _authService.Logout(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"{_username} logged out at {_timestampString}"));
        }

        [Fact]
        public void BillingView_UserSendsMessage_ShowsMessageCountForUser()
        {
            var messageArgs = new List<string>();
            _messagesMock.Setup(x => x.Add(Capture.In(messageArgs)));

            var billingService = new BillingAppService(_eventAggregator, _messagesMock.Object);
            _messageService.Send("Hello World", _username, "bob");
            _messageService.Send("Hello World", _username, "bob");
            _messageService.Send("Hello World", _username, "bob");

            Assert.Equal(new[]
            {
                $"{_username} has sent 1 message(s)",
                $"{_username} has sent 2 message(s)",
                $"{_username} has sent 3 message(s)"
            }, messageArgs);
        }

        [Fact]
        public void UsersView_OtherUserLogsIn_ShowsUserLogin()
        {
            var userService = new UserAppService(_eventAggregator, _messagesMock.Object);
            _authService.Login(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"User {_username} logged in"));
        }

        [Fact]
        public void UsersView_OtherUserLogsOut_ShowsUserLogout()
        {
            var userService = new UserAppService(_eventAggregator, _messagesMock.Object);
            _authService.Logout(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"User {_username} logged out"));
        }

        [Fact]
        public void UsersView_OtherSendsMessage_ShowsMessage()
        {
            var userService = new UserAppService(_eventAggregator, _messagesMock.Object);
            _messageService.Send("Hello World", _username, "bob");
            _messagesMock.Verify(x => x.Add($"{_username}: Hello World"));
        }

        [Fact]
        public void MonitoringView_UsersLogInAndOut_CountsNumberOfLoggedInUsers()
        {
            var messageArgs = new List<string>();
            _messagesMock.Setup(x => x.Add(Capture.In(messageArgs)));

            var monitoringService = new MonitoringAppService(_eventAggregator, _messagesMock.Object);
            
            _authService.Login(_username + "1", _timestamp);
            _authService.Login(_username + "1", _timestamp);
            _authService.Login(_username + "2", _timestamp);
            _authService.Login(_username + "3", _timestamp);
            _authService.Logout(_username + "1", _timestamp);

            Assert.Equal(new[]
            {
                "1 user(s) online",
                "1 user(s) online",
                "2 user(s) online",
                "3 user(s) online",
                "2 user(s) online"
            }, messageArgs);
        }

        [Fact]
        public void EventAggregatorPublish_SubscriberIsDisposed_RemovesSubscriber()
        {
            using (var subscriberStub = new DisposableSubscriber()) {
                _eventAggregator.Subscribe(subscriberStub);
                subscriberStub.Dispose();
            }

            GC.Collect();

            _eventAggregator.Publish("Hello World");
            var subscribers = _eventAggregator.GetSubscribers(typeof(ISubscriber<string>));

            Assert.Empty(subscribers);
        }
    }

    public class DisposableSubscriber : ISubscriber<string>, IDisposable
    {
        public void Dispose()
        {
        }

        public void Consume(string message)
        {
        }
    }
}
