using Prism.Events;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class UserAppService :
        ISubscriber<LoginMessage>,
        ISubscriber<LogoutMessage>,
        ISubscriber<UserMessage>
    {
        private readonly IMessageView _messagesView;

        public UserAppService(IEventAggregator eventAggregator, IMessageView messagesView)
        {
            _messagesView = messagesView;
            eventAggregator.GetEvent<LoginMessageEvent>().Subscribe(Consume);
            eventAggregator.GetEvent<LogoutMessageEvent>().Subscribe(Consume);
            eventAggregator.GetEvent<UserMessageEvent>().Subscribe(Consume);
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

        public void Consume(UserMessage message)
        {
            var messageString = $"{message.From}: {message.Message}";
            _messagesView.Add(messageString);
        }
    }
}