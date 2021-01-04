using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ConsoleClient
{
    public class SendMessage
    {
        public static void Send()
        {
            var msg = Console.ReadLine();
            if ((msg == "/d all" || Regex.IsMatch(msg,@"^\/d [0-9]+$")) && Program.role == "admin")
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, Program.appPath + "/chat");
                if (Regex.IsMatch(msg,@"^\/d [0-9]+$"))
                {
                    var id = Int32.Parse(msg.Split()[1]);
                    request.RequestUri = new Uri(Program.appPath + $"/chat/{id}");
                }
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.token);
                Program.client.SendAsync(request);
            }
            else
            {
                var json = JsonConvert.SerializeObject(new Message(msg, Program.username, DateTime.Now));
                var request = new HttpRequestMessage(HttpMethod.Post, Program.appPath + "/chat");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Program.token);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                Program.client.SendAsync(request);
            }
        }
    }
}