using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Online_Cinema_BLL.Infrastructure;
using Online_Cinema_BLL.Infrastructure.Provider;
using Online_Cinema_BLL.Services;
using Online_Cinema_BLL.Services.Interfaces;
using Online_Cinema_UI.Middlewares;
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
            BllConfiguration.Configuration(services, Configuration.GetConnectionString("MyCon"));
            //var res = Configuration["InstrumentationKey"];
            //BllConfiguration.Configuration(services, Configuration["ConnectionString"]);


            var option = new SendGridOptions()/* { SendGridKey = Configuration["SendGridKey"], UserSender = Configuration["UserSender"] }*/;

            Configuration.GetSection("SendGridOptions").Bind(option);
            services.AddTransient<SendGridOptions>(x => option);
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<MoviesService>();
            services.AddTransient<AdminService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddApplicationInsightsTelemetry(Configuration["InstrumentationKey"]);

            services.Configure<EmailConfirmationProviderOption>(op => op.TokenLifespan = TimeSpan.FromDays(5));

            services.AddAuthentication().AddCookie(op => op.LoginPath = "/Login");
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddTransient<ObserversManagementService>();
            //services.AddTransient<SignalRObserver>();
            services.AddControllersWithViews();
            services.AddSignalR();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<Chat>("/chat");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}