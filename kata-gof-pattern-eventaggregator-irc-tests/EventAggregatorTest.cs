using System;
using kata_gof_pattern_eventaggregator_irc;
using Moq;
using Xunit;

namespace kata_gof_pattern_eventaggregator_irc_tests
{
    public class EventAggregatorTest
    {
        private readonly DateTime _timestamp;
        private readonly string _username;
        private readonly Mock<IMessageView> _messagesMock;
        private readonly BillingAppService _billingService;
        private readonly AuthenticationAppService _authService;
        private readonly string _timestampString;

        public EventAggregatorTest()
        {
            _timestamp = DateTime.Now;
            _timestampString = _timestamp.ToString(Settings.TimeStampFormat);

            _username = "username";
            _messagesMock = new Mock<IMessageView>();
            var eventAggregator = new EventAggregator();

            _billingService = new BillingAppService(eventAggregator, _messagesMock.Object);
            _authService = new AuthenticationAppService(eventAggregator);
        }

        [Fact]
        public void BillingView_UserLogsIn_ShowsLoginTimestamp()
        {
            _authService.Login(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"{_username} logged in at {_timestampString}"));
        }

        [Fact]
        public void BillingView_UserLogsOut_ShowsLogoutTimestamp()
        {
            _authService.Logout(_username, _timestamp);
            _messagesMock.Verify(x => x.Add($"{_username} logged out at {_timestampString}"));
        }
    }
}
