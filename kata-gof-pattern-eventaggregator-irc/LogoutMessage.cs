using System;

namespace kata_gof_pattern_eventaggregator_irc
{
    public class LogoutMessage
    {
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }
    }
}