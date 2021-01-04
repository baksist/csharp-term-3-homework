using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleClient
{
    class Program
    {
        public static string AppPath = "http://localhost:5000";
        public static string Token;
        public static readonly HttpClient Client = new HttpClient();
        public static string Username;
        public static string Password;
        public static string Role;
        
        static void Main(string[] args)
        {
            if (args.Length == 2) 
                Config.SetHost(args[0], args[1]);

            Config.Greeter();

            Authenticate(Username, Password);

            var activityThread = new Thread(Activity.Update) {Name = "activityThread"};
            activityThread.Start();

            var messageThread = new Thread(MessageHistory.Update) {Name = "messagesThread"};
            messageThread.Start();

            while (true)
            {
                SendMessage.Send();
            }
        }

        private static void Authenticate(string newUsername, string newPassword)
        {
            var requestData = new
            {
                username = newUsername,
                password = newPassword
            };
            var response = Client.PostAsJsonAsync(AppPath + "/users/token", requestData);
            var rep = response.Result.Content.ReadAsStringAsync().Result;
            Token = JsonConvert.DeserializeObject<Dictionary<string, string>>(rep)["access_token"];
            Role = JsonConvert.DeserializeObject<Dictionary<string, string>>(rep)["role"];
        }

        public static void Login()
        {
            Console.Write("Enter login: ");
            Username = Console.ReadLine();
            Console.Write("Enter password: ");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                Password += key.KeyChar;
            }
        }
    }
}