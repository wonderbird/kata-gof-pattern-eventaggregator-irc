using System;
using System.Collections.Generic;
using kata_gof_pattern_eventaggregator_irc;
using Moq;
using Prism.Events;
using Unity;
using Xunit;

namespace kata_gof_pattern_eventaggregator_irc_tests
{
    public class EventAggregatorTest
    {
        public EventAggregatorTest()
        {
            _timestamp = DateTime.Now;
            _timestampString = _timestamp.ToString(Settings.TimeStampFormat);

            _username = "username";

            _messagesMock = new Mock<IMessageView>();
            _messageArgs = new List<string>();
            _messagesMock.Setup(x => x.Add(Capture.In(_messageArgs)));

            var eventAggregator = new EventAggregator();
            
            _container = new UnityContainer();
            _container.RegisterInstance<IEventAggregator>(eventAggregator, InstanceLifetime.Singleton);
            _container.RegisterInstance<IMessageView>(_messagesMock.Object, InstanceLifetime.Singleton);

            _authService = _container.Resolve<AuthenticationAppService>();
            _messageService = _container.Resolve<MessageAppService>();
        }

        private readonly DateTime _timestamp;
        private readonly string _timestampString;
        private readonly string _username;
        private readonly AuthenticationAppService _authService;
        private readonly Mock<IMessageView> _messagesMock;
        private readonly MessageAppService _messageService;
        private readonly List<string> _messageArgs;
        private readonly UnityContainer _container;

        [Fact]
        public void BillingView_UserLogsIn_ShowsLoginTimestamp()
        {
            var billingService = _container.Resolve<BillingAppService>();
            _authService.Login(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"{_username} logged in at {_timestampString}"));
        }

        [Fact]
        public void BillingView_UserLogsOut_ShowsLogoutTimestamp()
        {
            var billingService = _container.Resolve<BillingAppService>();
            _authService.Logout(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"{_username} logged out at {_timestampString}"));
        }

        [Fact]
        public void BillingView_UserSendsMessage_ShowsMessageCountForUser()
        {
            var billingService = _container.Resolve<BillingAppService>();

            _messageService.Send("Hello World", _username, "bob");
            _messageService.Send("Hello World", _username, "bob");
            _messageService.Send("Hello World", _username, "bob");

            Assert.Equal(new[]
            {
                $"{_username} has sent 1 message(s)",
                $"{_username} has sent 2 message(s)",
                $"{_username} has sent 3 message(s)"
            }, _messageArgs);
        }

        [Fact]
        public void MonitoringView_UsersLogInAndOut_CountsNumberOfLoggedInUsers()
        {
            var monitoringService = _container.Resolve<MonitoringAppService>();

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
            }, _messageArgs);
        }

        [Fact]
        public void UsersView_OtherSendsMessage_ShowsMessage()
        {
            var userService = _container.Resolve<UserAppService>();
            _messageService.Send("Hello World", _username, "bob");
            _messagesMock.Verify(x => x.Add($"{_username}: Hello World"));
        }

        [Fact]
        public void UsersView_OtherUserLogsIn_ShowsUserLogin()
        {
            var userService = _container.Resolve<UserAppService>();
            _authService.Login(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"User {_username} logged in"));
        }

        [Fact]
        public void UsersView_OtherUserLogsOut_ShowsUserLogout()
        {
            var userService = _container.Resolve<UserAppService>();
            _authService.Logout(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"User {_username} logged out"));
        }
    }
}