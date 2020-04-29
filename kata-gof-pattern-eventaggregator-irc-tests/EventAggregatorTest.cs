using System;
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

        public EventAggregatorTest()
        {
            _timestamp = DateTime.Now;
            _timestampString = _timestamp.ToString(Settings.TimeStampFormat);

            _username = "username";
            _messagesMock = new Mock<IMessageView>();
            _eventAggregator = new EventAggregator();

            _authService = new AuthenticationAppService(_eventAggregator);
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

    }
}
