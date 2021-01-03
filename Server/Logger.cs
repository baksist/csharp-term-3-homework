using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Server.Models;

namespace Server
{
    public class Logger
    {
        public const int LogSavePeriod = 5000;
        public const int UsersSavePeriod = 10000;
        public const int MaxMessageCount = 500;
        public const int DeleteMessagesCount = 100;
        public const string LogPath = "Utils/messageLog.json";
        public const string UsersPath = "Utils/users.json";

        public static void SaveLog()
        {
            var writer = new StreamWriter(LogPath, false);
            writer.Write(JsonConvert.SerializeObject(Program.Messages));
            writer.Close();
        }

        public static void LoadLog()
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

        public static void SaveUsers()
        {
            var writer = new StreamWriter(UsersPath, false);
            writer.Write(JsonConvert.SerializeObject(Program.Users));
            writer.Close();
        }

        public static void LoadUsers()
        {
            try
            {
                var reader = new StreamReader(UsersPath);
                Program.Users = JsonConvert.DeserializeObject<List<ChatUser>>(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Users' data not found");
            }
        }
    }
}