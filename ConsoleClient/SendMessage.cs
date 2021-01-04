using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ConsoleClient
{
    public static class SendMessage
    {
        public static void Send()
        {
            var msg = Console.ReadLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            ClearCurrentConsoleLine();
            if ((msg == "/d all" || Regex.IsMatch(msg,@"^\/d [0-9]+$")) && Program.Role == "admin")
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, Program.AppPath + "/chat");
                if (Regex.IsMatch(msg,@"^\/d [0-9]+$"))
                {
                    var id = Int32.Parse(msg.Split()[1]);
                    request.RequestUri = new Uri(Program.AppPath + $"/chat/{id}");
                }
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.Token);
                Program.Client.SendAsync(request);
            }
            else
            {
                var json = JsonConvert.SerializeObject(new Message(msg, Program.Username, DateTime.Now));
                var request = new HttpRequestMessage(HttpMethod.Post, Program.AppPath + "/chat");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.Token);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                Program.Client.SendAsync(request);
            }
        }
        private static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}