using System;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class AuthenticationAppService
    {
        private readonly EventAggregator _eventAggregator;

        public AuthenticationAppService(EventAggregator eventAggregator)
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
            _eventAggregator.Publish(message);
        }
    }
}