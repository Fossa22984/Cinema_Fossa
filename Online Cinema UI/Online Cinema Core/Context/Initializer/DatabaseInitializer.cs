using Microsoft.AspNetCore.Identity;
using Online_Cinema_Core.Interface;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using OnlineCinema_Core.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Online_Cinema_Core.Context.Initializer
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        OnlineCinemaContext _onlineCinemaContext;
        UserManager<User> _userManager;
        public DatabaseInitializer(OnlineCinemaContext onlineCinemaContext, UserManager<User> userManager)
        {
            _onlineCinemaContext = onlineCinemaContext;
            _userManager = userManager;
        }

        public void Initialize()
        {
            _onlineCinemaContext.Database.EnsureCreated();

            #region Add Role (Admin)
            if (!_onlineCinemaContext.Roles.Any())
            {
                _onlineCinemaContext.Roles.Add(new Role() { Name = "Admin", NormalizedName = "Admin" });
                _onlineCinemaContext.Roles.Add(new Role() { Name = "User", NormalizedName = "User" });
                _onlineCinemaContext.SaveChanges();
            }
            #endregion

            #region Add Default Admin
            if (!_userManager.Users.Where(x => x.UserName == "Admin").Any())
            {
                var admin = new User() { Email = "my.code.fossa@gmail.com", EmailConfirmed = true, UserName = "Admin" };
                var fileInfo = new FileInfo(DefaultRootHelper.Current.DefaultIconPath);
                if (fileInfo.Length > 0)
                {
                    admin.Photo = new byte[fileInfo.Length];
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        fs.Read(admin.Photo, 0, admin.Photo.Length);
                    }
                }
                _userManager.CreateAsync(admin, "31415926535@qAZ").Wait();
                _userManager.AddToRolesAsync(admin, new List<string>() { "Admin", "User" }).Wait();
            }
            #endregion

            #region Add Genres
            if (!_onlineCinemaContext.Genres.Any())
            {
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Артхаус" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Биографический" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Боевик" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Вестерн" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Военный" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Детектив" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Детский" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Документальный" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Драма" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Исторический" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Кинокомикс" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Комедия" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Короткометражный" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Криминал" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Мелодрама" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Мистика" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Немое кино" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Приключения" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Триллер" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Ужасы" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Фантастика" });
                _onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Фэнтези" });

                _onlineCinemaContext.SaveChanges();
            }
            #endregion
        }
    }
}
