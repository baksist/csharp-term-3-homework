using System;

namespace ConsoleClient
{
    public class Message
    {
        public string Text { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        
        public Message(string text, string username, DateTime date)
        {
            Text = text;
            Username = username;
            Date = date;
        }

        public override string ToString()
        {
            return $"[{Date.ToString("g")}] {Username}: {Text}";
        }
    }
}