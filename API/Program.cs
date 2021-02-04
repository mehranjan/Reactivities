using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; //UseStartup
using Microsoft.EntityFrameworkCore; //Migrate
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; //CreateScope
using Microsoft.Extensions.Hosting; //IHostBuilder
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider; //Get a reference to out services
                try{
                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                }catch(Exception ex){
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex,"An error occured during migrations");
                }

            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
