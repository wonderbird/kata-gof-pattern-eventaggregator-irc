namespace kata_gof_pattern_eventaggregator_irc
{
    public interface ISubscriber<in T>
    {
        void Consume(T message);
    }
}