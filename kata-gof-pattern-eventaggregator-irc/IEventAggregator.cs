namespace kata_gof_pattern_eventaggregator_irc
{
    public interface IEventAggregator
    {
        void Subscribe(object subscriber);
        void Publish<T>(T message);
    }
}