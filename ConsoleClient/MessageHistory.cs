using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace ConsoleClient
{
    public class MessageHistory
    {
        public static int UpdatePeriod = 200;
        public static void Update()
        {
            var response = Program.Client.GetAsync(Program.AppPath + "/chat");
            var Messages =
                JsonConvert.DeserializeObject<List<Message>>(response.Result.Content.ReadAsStringAsync().Result);
            foreach (var msg in Messages)
            {
                Console.WriteLine($"#{Messages.FindIndex(x => x == msg)}, {msg}");
            }
            
            while (true)
            {
                response = Program.Client.GetAsync(Program.AppPath + "/chat");
                var newMessages = 
                    JsonConvert.DeserializeObject<List<Message>>(response.Result.Content.ReadAsStringAsync().Result);
                if (newMessages.Count > Messages.Count)
                {
                    var messagesToAdd = new List<Message>();
                    for (var i = 0; i < newMessages.Count; i++)
                    {
                        if (i >= Messages.Count)
                        {
                            Console.WriteLine($"#{i}, {newMessages[i]}");
                            messagesToAdd.Add(newMessages[i]);
                        }
                    }
                    Messages.AddRange(messagesToAdd);
                    messagesToAdd.Clear();
                }
                else if (newMessages.Count < Messages.Count)
                {
                    Console.Clear();
                    Messages = newMessages;
                    foreach (var msg in Messages)
                    {
                        Console.WriteLine($"#{Messages.FindIndex(x => x == msg)}, {msg}");
                    }
                }
                Thread.Sleep(UpdatePeriod);
            }
        }
    }
}