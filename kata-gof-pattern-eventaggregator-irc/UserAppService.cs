namespace kata_gof_pattern_eventaggregator_irc
{
    public class UserAppService : ISubscriber<LoginMessage>, ISubscriber<LogoutMessage>
    {
        private readonly IMessageView _messagesView;

        public UserAppService(EventAggregator eventAggregator, IMessageView messagesView)
        {
            _messagesView = messagesView;
            eventAggregator.Subscribe(this);
        }

        public void Consume(LoginMessage message)
        {
            var messageString = $"User {message.Username} logged in";
            _messagesView.Add(messageString);
        }

        public void Consume(LogoutMessage message)
        {
            var messageString = $"User {message.Username} logged out";
            _messagesView.Add(messageString);
        }
    }
}