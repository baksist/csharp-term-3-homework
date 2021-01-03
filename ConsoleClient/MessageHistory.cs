using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace ConsoleClient
{
    public class MessageHistory
    {
        public static void Update()
        {
            while (true)
            {
                var response = Program.client.GetAsync(Program.appPath + "/chat");
                var Messages =
                    JsonConvert.DeserializeObject<List<Message>>(response.Result.Content.ReadAsStringAsync().Result);
                foreach (var msg in Messages)
                {
                    Console.WriteLine(msg);
                }
                
                Thread.Sleep(200);
            }
        }
    }
}