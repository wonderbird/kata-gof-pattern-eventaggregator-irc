namespace kata_gof_pattern_eventaggregator_irc
{
    public class MessageAppService
    {
        private readonly EventAggregator _eventAggregator;

        public MessageAppService(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Send(string message, string from, string to)
        {
            _eventAggregator.Publish(new UserMessage {From = from, To = to, Message = message});
        }
    }
}