namespace kata_gof_pattern_eventaggregator_irc
{
    public class BillingAppService : ISubscriber<LoginMessage>, ISubscriber<LogoutMessage>
    {
        private readonly IMessageView _messageView;

        public BillingAppService(EventAggregator eventAggregator, IMessageView messageView)
        {
            eventAggregator.Subscribe(this);
            //eventAggregator.Subscribe<LoginMessage>(this);
            //eventAggregator.Subscribe<LogoutMessage>(this);
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
    }
}