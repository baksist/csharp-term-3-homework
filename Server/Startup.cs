using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Server.Models;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            var logThread = new Thread(SaveLog) {Name = "logThread"};
            logThread.Start();
            
            var usersThread = new Thread(SaveUsers) {Name = "usersThread"};
            usersThread.Start();

            var activityThread = new Thread(ActivityMonitor) {Name = "activityThread"};
            activityThread.Start();
        }

        public IConfiguration Configuration { get; }
        
        /// <summary>
        /// Function that saves message history to a file every N milliseconds.
        /// N is specified as LogSavePeriod in Logger class.
        /// </summary>
        public static void SaveLog()
        {
            while (true)
            {
                if (Program.Messages.Count > Logger.MaxMessageCount)
                {
                    Program.Messages.RemoveRange(0, Logger.DeleteMessagesCount);
                }
                Logger.SaveLog();
                Thread.Sleep(Logger.LogSavePeriod);
            }
        }

        /// <summary>
        /// Function that saves user info to a file every N milliseconds.
        /// N is specified as UsersSavePeriod in Logger class
        /// </summary>
        public static void SaveUsers()
        {
            while (true)
            {
                Logger.SaveUsers();
                Thread.Sleep(Logger.UsersSavePeriod);
            }
        }

        /// <summary>
        /// Function that monitors user activity.
        /// If a user is inactive for more that TimeoutSeconds, they will be deleted from ActiveUsers list.
        /// </summary>
        public static void ActivityMonitor()
        {
            var removeList = new List<ActiveUser>();
            while (true)
            {
                foreach (var user in Program.ActiveUsers)
                {
                    if (user.LastSeen.AddSeconds(ActiveUser.TimeoutSeconds) < DateTime.Now)
                    {
                        Program.Messages.Add(new Message($"{user.Username} disconnected", "system", DateTime.Now));
                        removeList.Add(user);
                    }
                }

                foreach (var user in removeList)
                {
                    Program.ActiveUsers.Remove(user);
                }
                
                Thread.Sleep(ActiveUser.MonitorPeriod);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = TokenOptions.ISSUER,
                        
                        ValidateAudience = true,
                        ValidAudience = TokenOptions.AUDIENCE,
                        
                        ValidateLifetime = true,
                        
                        IssuerSigningKey = TokenOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        
                    };
                });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}