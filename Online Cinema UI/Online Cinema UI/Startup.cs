using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Online_Cinema_BLL.Infrastructure;
using Online_Cinema_BLL.Infrastructure.Provider;
using Online_Cinema_BLL.SignalR;
using Online_Cinema_Config.AppStart;
using Online_Cinema_UI.Filters;
using OnlineCinema_Core.Helpers;
using System;

namespace Online_Cinema_UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var option = new SendGridOptions();

            Configuration.GetSection("SendGridOptions").Bind(option);
            services.AddTransient<SendGridOptions>(x => option);

            DIRegistration.RegisterConfigs(ref services, Configuration.GetConnectionString("MyCon"));

            services.AddApplicationInsightsTelemetry(Configuration["InstrumentationKey"]);

            services.Configure<EmailConfirmationProviderOption>(op => op.TokenLifespan = TimeSpan.FromDays(5));

            services.AddAuthentication().AddCookie(op => op.LoginPath = "/Login");

            services.AddControllersWithViews();
            services.AddSignalR();


            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = SettingsHelper.Current.MaxRequestLenghts;
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = SettingsHelper.Current.MaxRequestLenghts;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Account/Error");
            }
            app.UseStaticFiles();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<RoomHub>("/chat");
                endpoints.MapHub<NotificationHub>("/notification");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
