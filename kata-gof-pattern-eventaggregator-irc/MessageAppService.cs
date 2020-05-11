namespace kata_gof_pattern_eventaggregator_irc
{
    public class MessageAppService
    {
        private readonly IEventAggregator _eventAggregator;

        public MessageAppService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Send(string message, string from, string to)
        {
            _eventAggregator.Publish(new UserMessage {From = from, To = to, Message = message});
        }
    }
}