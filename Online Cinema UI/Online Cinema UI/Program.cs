using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Online_Cinema_BLL.Interfaces.Observers;
using Online_Cinema_Core.Interface;
using OnlineCinema_Core.Helpers;

namespace Online_Cinema_UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var observerPool = services.GetRequiredService<IObserversPoolManager>();
            observerPool.Start();

            var dataBaseInitilizer = services.GetRequiredService<IDatabaseInitializer>();
            dataBaseInitilizer.Initialize();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, config) => { }).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>().ConfigureKestrel(serverOptions =>
                {
                    serverOptions.Limits.MaxRequestBodySize = SettingsHelper.Current.MaxRequestLenghts;
                });
            });
    }
}