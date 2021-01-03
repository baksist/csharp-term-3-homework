using System;
using System.Collections.Generic;
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
        
        static void Main(string[] args)
        {
           /*Console.Write("Enter login: ");
            var username = Console.ReadLine();
            
            Console.Write("Enter password: ");
            string password = null;
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
            }
*/
            var username = "new_user";
            var password = "12345";
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
    }
}