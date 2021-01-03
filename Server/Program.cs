using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Server.Models;
using Server.Utils;

namespace Server
{
    public class Program
    {
        /// <summary>
        /// List with message history.
        /// </summary>
        public static List<Message> Messages = new List<Message>();
        
        /// <summary>
        /// List with users and their roles
        /// </summary>
        public static List<ChatUser> Users = new List<ChatUser>();
        
        /// <summary>
        /// List with active users.
        /// </summary>
        public static List<ActiveUser> ActiveUsers = new List<ActiveUser>();
        private static string customURL = "http://localhost:5000";
        
        public static void Main(string[] args)
        {
            
            if (args.Length == 2 && RegexUtils.MatchParams(args[0], args[1]))
            {
                var IP = args[0];
                var port = args[1];
                customURL = $"http://{IP}:{port}";
            }
            Logger.LoadLog();
            Logger.LoadUsers();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(customURL);
                });
    }
}