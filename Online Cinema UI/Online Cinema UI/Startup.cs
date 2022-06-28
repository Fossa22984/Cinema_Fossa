using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Online_Cinema_BLL.Infrastructure;
using Online_Cinema_BLL.Infrastructure.Provider;
using Online_Cinema_BLL.Managers;
using Online_Cinema_BLL.Services;
using Online_Cinema_BLL.Services.Interfaces;
using Online_Cinema_BLL.SignalR;
using Online_Cinema_BLL.Ñache;
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
            services.AddTransient<UploadFileAzureManager>();
            services.AddTransient<FileManager>();
            services.AddSingleton<NotificationHub>();
            services.AddSingleton<NotificationCache>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddApplicationInsightsTelemetry(Configuration["InstrumentationKey"]);

            services.Configure<EmailConfirmationProviderOption>(op => op.TokenLifespan = TimeSpan.FromDays(5));

            services.AddAuthentication().AddCookie(op => op.LoginPath = "/Login");
            //services.AddMvc()
            //    .AddRazorPagesOptions(options =>
            //    {
            //        options.Conventions
            //            .AddPageApplicationModelConvention("/FileUploadPage",
            //                model =>
            //                {
            //        // Handle requests up to 50 MB
            //        model.Filters.Add(
            //                        new RequestSizeLimitAttribute(52428800));
            //                });
            //    })
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddTransient<ObserversManagementService>();
            //services.AddTransient<SignalRObserver>();
            services.AddControllersWithViews();
            services.AddSignalR();

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 2147483648;
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 2147483648;
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<NotificationHub>("/notification");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
