using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Server
{
    public class Logger
    {
        public const int SavePeriod = 5000;
        public const string LogPath = "Utils/messageLog.json";

        public static void Save()
        {
            var writer = new StreamWriter(LogPath, false);
            writer.Write(JsonConvert.SerializeObject(Program.Messages));
            writer.Close();
        }

        public static void Load()
        {
            try
            {
                var reader = new StreamReader(LogPath);
                Program.Messages = JsonConvert.DeserializeObject<List<Message>>(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Log file not found, message history is unavailable");
            }
            
        }
    }
}