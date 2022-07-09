using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Online_Cinema_BLL.Cache;
using Online_Cinema_BLL.Interfaces.Cache;
using Online_Cinema_BLL.Interfaces.Managers;
using Online_Cinema_BLL.Interfaces.Observers;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_BLL.Observers;
using Online_Cinema_BLL.Observers.Base;
using Online_Cinema_BLL.Services;
using Online_Cinema_BLL.Services.Managers;
using Online_Cinema_BLL.SignalR;
using Online_Cinema_Core.Context;
using Online_Cinema_Core.Context.Initializer;
using Online_Cinema_Core.Interface;
using Online_Cinema_Core.Settings.Interfaces;
using Online_Cinema_Core.Settings.Managers;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models.IdentityModels;

namespace Online_Cinema_Config.AppStart
{
    public class DIRegistration
    {
        public static void RegisterConfigs(ref IServiceCollection services, string dbConnection)
        {
            //Mapper
            services.AddSingleton(AutoMapperProfile.GetMapper());

            //Services
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IMoviesService, MoviesService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<ICinemaRoomService, CinemaRoomService>();

            //Managers
            services.AddTransient<IUploadFileAzureManager, UploadFileAzureManager>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<IAzureSettingsManager, AzureSettingsManager>();

            //SignalR
            services.AddSingleton<NotificationHub>();
            services.AddSingleton<ChatHub>();

            //Observers
            services.AddSingleton<IObserversPoolManager, ObserversPoolManager>();
            services.AddSingleton<IGetSessionObserver, GetSessionObserver>();

            //Cache
            services.AddSingleton<ICinemaRoomCacheManager, CinemaRoomCacheManager>();
            services.AddSingleton<ISessionCacheManager, SessionCacheManager>();
            services.AddSingleton<INotificationCacheManager, NotificationCacheManager>();

            // Domain
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddDbContext<OnlineCinemaContext>(x => x.UseSqlServer(dbConnection));
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<OnlineCinemaContext>().AddDefaultTokenProviders();
        }
    }
}
