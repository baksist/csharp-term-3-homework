using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Server.Models;

namespace Server
{
    public class Program
    {
        public static List<Message> Messages = new List<Message>();
        public static List<ChatUser> Users = new List<ChatUser>();
        
        public static void Main(string[] args)
        {
            Logger.Load();
            Users.Add(new ChatUser());
            Users.Add(new ChatUser{ Username = "admin1337", Password = "kek", Role = "admin" });
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}