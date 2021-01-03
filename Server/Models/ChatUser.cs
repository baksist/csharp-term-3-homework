﻿namespace Server.Models
{
    /// <summary>
    /// Class representing a user
    /// </summary>
    public class ChatUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public ChatUser()
        {
            Username = "DefaultUser";
            Password = "qwerty";
            Role = "user";
        }
    }
}