using System.Collections.Generic;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class MonitoringAppService : ISubscriber<LoginMessage>, ISubscriber<LogoutMessage>
    {
        private readonly IMessageView _messagesView;
        private readonly HashSet<string> _loggedInUsers = new HashSet<string>();

        public MonitoringAppService(EventAggregator eventAggregator, IMessageView messagesView)
        {
            _messagesView = messagesView;
            eventAggregator.Subscribe(this);
        }

        public void Consume(LoginMessage message)
        {
            _loggedInUsers.Add(message.Username);
            _messagesView.Add($"{_loggedInUsers.Count} user(s) online");
        }

        public void Consume(LogoutMessage message)
        {
            _loggedInUsers.Remove(message.Username);
            _messagesView.Add($"{_loggedInUsers.Count} user(s) online");
        }
    }
}