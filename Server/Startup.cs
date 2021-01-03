using System.Threading;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var logThread = new Thread(SaveLog) {Name = "logThread"};
            var usersThread = new Thread(SaveUsers) {Name = "usersThread"};

            logThread.Start();
            usersThread.Start();
        }

        public IConfiguration Configuration { get; }
        
        // TODO: clear log every N messages
        public static void SaveLog()
        {
            while (true)
            {
                Logger.SaveLog();
                Thread.Sleep(Logger.LogSavePeriod);
            }
        }

        public static void SaveUsers()
        {
            while (true)
            {
                Logger.SaveUsers();
                Thread.Sleep(Logger.UsersSavePeriod);
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