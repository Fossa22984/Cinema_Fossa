using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Online_Cinema_BLL.Observers.Base;

namespace Online_Cinema_UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;


            var observerPool = services.GetRequiredService<ObserversPoolManager>();
            observerPool.Start();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, config) =>
            {
            }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureKestrel(serverOptions =>
                                {
                                    serverOptions.Limits.MaxRequestBodySize = 2147483648;
                                });

                });
    }
}
