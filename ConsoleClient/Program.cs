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
        public static string appPath = "http://localhost:5000";
        public static string token;
        public static HttpClient client = new HttpClient();
        public static string username;
        public static string password;
        
        static void Main(string[] args)
        {
            /*
            if (File.Exists(Config.ConfigPath))
            {
                Console.WriteLine("Config file found! Do you want to load it? (y/n)");
                var c = Console.ReadLine();
                if (c == "y")
                    Config.Load();
                else
                {
                    Console.WriteLine("Ok, using default settings");
                    Login();
                }
            }
            else
            {
                Console.WriteLine("Config file not found! Do you want to create it? (y/n)");
                var c = Console.ReadLine();
                if (c == "y")
                    Config.Create();
                else
                {
                    Console.WriteLine("Ok, using default settings");
                    Login();
                }
            }
            */
            
            username = "new_user";
            password = "12345";
            
            Authenticate(username, password);

            var activityThread = new Thread(Activity.Update) {Name = "activityThread"};
            activityThread.Start();

            var messageThread = new Thread(MessageHistory.Update) {Name = "messagesThread"};
            messageThread.Start();

        }

        static void Authenticate(string _username, string _password)
        {
            var requestData = new
            {
                username = _username,
                password = _password
            };
            var response = client.PostAsJsonAsync(appPath + "/users/token", requestData);
            var rep = response.Result.Content.ReadAsStringAsync().Result;
            token = JsonConvert.DeserializeObject<Dictionary<string, string>>(rep)["access_token"];
        }

        public static void Login()
        {
            Console.Write("Enter login: ");
            username = Console.ReadLine();
            Console.Write("Enter password: ");
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
            }
        }
    }
}