using Prism.Events;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class BillingAppService :
        ISubscriber<LoginMessage>,
        ISubscriber<LogoutMessage>,
        ISubscriber<UserMessage>
    {
        private readonly IMessageView _messageView;
        private int _userMessageCount;

        public BillingAppService(IEventAggregator eventAggregator, IMessageView messageView)
        {
            eventAggregator.GetEvent<LoginMessageEvent>().Subscribe(Consume);
            eventAggregator.GetEvent<LogoutMessageEvent>().Subscribe(Consume);
            eventAggregator.GetEvent<UserMessageEvent>().Subscribe(Consume);
            _messageView = messageView;
        }

        public void Consume(LoginMessage message)
        {
            var timestampString = message.Timestamp.ToString(Settings.TimeStampFormat);
            var logString = $"{message.Username} logged in at {timestampString}";

            _messageView.Add(logString);
        }

        public void Consume(LogoutMessage message)
        {
            var timestampString = message.Timestamp.ToString(Settings.TimeStampFormat);
            var logString = $"{message.Username} logged out at {timestampString}";

            _messageView.Add(logString);
        }

        public void Consume(UserMessage message)
        {
            _userMessageCount++;
            _messageView.Add($"{message.From} has sent {_userMessageCount} message(s)");
        }
    }
}