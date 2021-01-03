using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Server.Models;

namespace Server
{
    // TODO: add activity controller
    // TODO: add IP and port settings
    public class Program
    {
        public static List<Message> Messages = new List<Message>();
        public static List<ChatUser> Users = new List<ChatUser>();
        
        public static void Main(string[] args)
        {
            Logger.LoadLog();
            Logger.LoadUsers();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}