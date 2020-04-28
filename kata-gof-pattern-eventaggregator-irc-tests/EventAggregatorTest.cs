using System;
using kata_gof_pattern_eventaggregator_irc;
using Moq;
using Xunit;

namespace kata_gof_pattern_eventaggregator_irc_tests
{
    public class EventAggregatorTest
    {
        [Fact]
        public void BillingView_UserLogsIn_ShowsLoginTimestamp()
        {
            var timestamp = DateTime.Now;
            var username = "username";

            var messagesMock = new Mock<IMessageView>();
            var eventAggregator = new EventAggregator();

            var billingService = new BillingAppService(eventAggregator, messagesMock.Object);

            var authService = new AuthenticationAppService(eventAggregator);
            authService.Login(username, timestamp);

            var timestampString = timestamp.ToString(Settings.TimeStampFormat);
            messagesMock.Verify(x => x.Add($"{username} logged in at {timestampString}"));
        }

        [Fact]
        public void BillingView_UserLogsOut_ShowsLogoutTimestamp()
        {
            var timestamp = DateTime.Now;
            var username = "username";

            var messagesMock = new Mock<IMessageView>();
            var eventAggregator = new EventAggregator();

            var billingService = new BillingAppService(eventAggregator, messagesMock.Object);

            var authService = new AuthenticationAppService(eventAggregator);
            authService.Logout(username, timestamp);

            var timestampString = timestamp.ToString(Settings.TimeStampFormat);
            messagesMock.Verify(x => x.Add($"{username} logged out at {timestampString}"));
        }
    }
}
