using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Online_Cinema_Core.Context;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models.IdentityModels;

namespace Online_Cinema_BLL.Infrastructure
{
    public static class BllConfiguration
    {
        public static void Configuration(IServiceCollection collection, string dbConnection)
        {
            collection.AddDbContext<OnlineCinemaContext>(x => x.UseSqlServer(dbConnection));
            collection.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<OnlineCinemaContext>().AddDefaultTokenProviders()/*.AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation")*/;

            collection.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}