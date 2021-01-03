using System;

namespace Server.Models
{
    /// <summary>
    /// Class representing active (online) user. 
    /// </summary>
    public class ActiveUser
    {
        public string Username { get; set; }
        public DateTime LastSeen { get; set; }
        
        /// <summary>
        /// Time period (in seconds) after which user will be considered offline.
        /// </summary>
        public const int TimeoutSeconds = 10;
        
        /// <summary>
        /// Time period (in milliseconds) after which the program will check activity of a user.
        /// </summary>
        public const int MonitorPeriod = 200;
    }
}