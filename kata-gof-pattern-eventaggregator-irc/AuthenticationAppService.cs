using System;
using Prism.Events;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class AuthenticationAppService
    {
        private readonly IEventAggregator _eventAggregator;

        public AuthenticationAppService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Login(string username, DateTime timestamp)
        {
            var message = new LoginMessage
            {
                Username = username,
                Timestamp = timestamp
            };
            _eventAggregator.GetEvent<LoginMessageEvent>().Publish(message);
        }

        public void Logout(string username, DateTime timestamp)
        {
            var message = new LogoutMessage
            {
                Username = username,
                Timestamp = timestamp
            };
            _eventAggregator.GetEvent<LogoutMessageEvent>().Publish(message);
        }
    }
}