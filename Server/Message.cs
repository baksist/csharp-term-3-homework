using System;

namespace Server
{
    public class Message
    {
        public string Text;
        public string Username;
        public DateTime Date;

        public Message()
        {
            Text = "Default text";
            Username = "Default user";
            Date = DateTime.Now;
        }

        public Message(string text, string username, DateTime date)
        {
            Text = text;
            Username = text;
            Date = date;
        }

        public override string ToString()
        {
            return Username + ": " + Text + " at " + Date;
        }
    }
}