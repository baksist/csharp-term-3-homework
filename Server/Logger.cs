using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Server.Models;

namespace Server
{
    /// <summary>
    /// class for logging message history and user information
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Time period (in ms) for message history saving
        /// </summary>
        public const int LogSavePeriod = 5000;
        
        /// <summary>
        /// Time period (in ms) for saving user info
        /// </summary>
        public const int UsersSavePeriod = 10000;
        
        /// <summary>
        /// Maximum amount of messages possiblle. If exceeded, amount of messages specified in MaxMessageCount will be deleted.
        /// </summary>
        public const int MaxMessageCount = 500;
        
        /// <summary>
        /// Amount of message to delete if MaxMessageCount is exceeded.
        /// </summary>
        public const int DeleteMessagesCount = 100;
        
        /// <summary>
        /// Path for message history file.
        /// </summary>
        public const string LogPath = "Utils/messageLog.json";
        
        /// <summary>
        /// Path for user info file.
        /// </summary>
        public const string UsersPath = "Utils/users.json";

        /// <summary>
        /// Function that serializes message history to JSON file.
        /// Path is specified in LogPath.
        /// </summary>
        public static void SaveLog()
        {
            var writer = new StreamWriter(LogPath, false);
            writer.Write(JsonConvert.SerializeObject(Program.Messages));
            writer.Close();
        }

        /// <summary>
        /// Function that loads message history from JSON file
        /// </summary>
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

        /// <summary>
        /// Function that saves user info to JSON file
        /// </summary>
        public static void SaveUsers()
        {
            var writer = new StreamWriter(UsersPath, false);
            writer.Write(JsonConvert.SerializeObject(Program.Users));
            writer.Close();
        }

        /// <summary>
        /// Function that loads from JSON file.
        /// </summary>
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