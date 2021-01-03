using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleClient
{
    public class Config
    {
        public const string ConfigPath = "config.json";

        public static void Load()
        {
            try
            {
                var reader = new StreamReader(ConfigPath);
                var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
                Program.username = properties["username"];
                Program.password = properties["password"];
                MessageHistory.UpdatePeriod = Int32.Parse(properties["message_update"]);
                Activity.ActivityPeriod = Int32.Parse(properties["online_update"]);
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
            }
        }

        public static void Create()
        {
            Program.Login();
            Console.Write("Enter message update period (in ms): ");
            MessageHistory.UpdatePeriod = Int32.Parse(Console.ReadLine());
            Console.Write("Enter online activity update period (in ms): ");
            Activity.ActivityPeriod = Int32.Parse(Console.ReadLine());

            var properties = new Dictionary<string, string>
            {
                {"username", Program.username},
                {"password", Program.password},
                {"message_update", MessageHistory.UpdatePeriod.ToString()},
                {"online_update", Activity.ActivityPeriod.ToString()}
            };
            
            var writer = new StreamWriter(ConfigPath, false);
            writer.Write(JsonConvert.SerializeObject(properties));
            writer.Close();
        }
    }
}