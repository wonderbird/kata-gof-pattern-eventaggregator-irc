namespace kata_gof_pattern_eventaggregator_irc
{
    public class BillingAppService : ISubscriber<LoginMessage>
    {
        private readonly IMessageView _messageView;

        public BillingAppService(EventAggregator eventAggregator, IMessageView messageView)
        {
            eventAggregator.Subscribe(this);
            _messageView = messageView;
        }

        public void Consume(LoginMessage message)
        {
            var timestampString = message.Timestamp.ToString(Settings.TimeStampFormat);
            var logString = $"{message.Username} logged in at {timestampString}";

            _messageView.Add(logString);
        }
    }
}