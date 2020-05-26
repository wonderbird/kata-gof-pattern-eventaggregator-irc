using Prism.Events;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class UserMessageEvent : PubSubEvent<UserMessage>
    {
    }
}