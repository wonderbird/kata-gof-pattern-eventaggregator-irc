namespace kata_gof_pattern_eventaggregator_irc
{
    public class MessageAppService
    {
        private readonly EventAggregator _eventAggregator;
        private readonly IMessageView _messageView;

        public MessageAppService(EventAggregator eventAggregator, IMessageView messageView)
        {
            _eventAggregator = eventAggregator;
            _messageView = messageView;
        }

        public void Send(string message, string from, string to)
        {
            _eventAggregator.Publish(new UserMessage { From = from, To = to, Message = message });
        }
    }
}