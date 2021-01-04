using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleClient
{
    public static class Config
    {
        private const string ConfigPath = "config.json";

        private static void Load()
        {
            try
            {
                var reader = new StreamReader(ConfigPath);
                var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
                Program.Username = properties["username"];
                Program.Password = properties["password"];
                MessageHistory.UpdatePeriod = Int32.Parse(properties["message_update"]);
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("error");
            }
        }

        private static void Create()
        {
            Program.Login();
            Console.Write("Enter message update period (in ms): ");
            MessageHistory.UpdatePeriod = Int32.Parse(Console.ReadLine());

            var properties = new Dictionary<string, string>
            {
                {"username", Program.Username},
                {"password", Program.Password},
                {"message_update", MessageHistory.UpdatePeriod.ToString()},
            };
            
            var writer = new StreamWriter(ConfigPath, false);
            writer.Write(JsonConvert.SerializeObject(properties));
            writer.Close();
        }

        public static void Greeter()
        {
            if (File.Exists(ConfigPath))
            {
                Console.WriteLine("Config file found! Do you want to load it? (y/n)");
                var c = Console.ReadLine();
                if (c == "y")
                    Load();
                else
                {
                    Console.WriteLine("Ok, using default settings");
                    Program.Login();
                }
            }
            else
            {
                Console.WriteLine("Config file not found! Do you want to create it? (y/n)");
                var c = Console.ReadLine();
                if (c == "y")
                    Create();
                else
                {
                    Console.WriteLine("Ok, using default settings");
                    Program.Login();
                }
            }
        }
    }
}