using System;

namespace Server.Models
{
    public class ActiveUser
    {
        public string Username { get; set; }
        public DateTime LastSeen { get; set; }

        public const int TimeoutSeconds = 10;
        public const int MonitorPeriod = 200;
    }
}