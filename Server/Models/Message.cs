using System;

namespace Server.Models
{
    [Serializable]
    public class Message
    {
        public string Text { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }

        public Message()
        {
            Text = "Default text";
            Username = "Default user";
            Date = DateTime.Now;
        }

        public Message(string text, string username, DateTime date)
        {
            Text = text;
            Username = username;
            Date = date;
        }
    }
}