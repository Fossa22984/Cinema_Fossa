using Microsoft.AspNetCore.Identity;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using System.Linq;

namespace Online_Cinema_Core.Context.Initializer
{
    public static class ModelInitializer
    {
        public static void Initializer(OnlineCinemaContext onlineCinemaContext/*, UserManager<User> _userManager*/)
        {
            onlineCinemaContext.Database.EnsureCreated();

            #region Add Role (Admin)
            if (!onlineCinemaContext.Roles.Any())
            {
                onlineCinemaContext.Roles.Add(new Role() { Name = "Admin", NormalizedName = "Admin" });
                onlineCinemaContext.Roles.Add(new Role() { Name = "User", NormalizedName = "User" });
                onlineCinemaContext.SaveChanges();
            }
            #endregion
            #region Add Genres
            if (!onlineCinemaContext.Genres.Any())
            {
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Артхаус" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Биографический" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Боевик" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Вестерн" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Военный" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Детектив" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Детский" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Документальный" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Драма" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Исторический" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Кинокомикс" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Комедия" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Короткометражный" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Криминал" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Мелодрама" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Мистика" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Немое кино" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Приключения" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Триллер" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Ужасы" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Фантастика" });
                onlineCinemaContext.Genres.Add(new Genre() { GenreName = "Фэнтези" });

                onlineCinemaContext.SaveChanges();
            }
            #endregion


            #region Add Movies
            //for (int i = 0; i < 10; i++)
            //{



            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://media.kg-portal.ru/movies/s/sawlegacy/posters/sawlegacy_35.jpg",
            //        FilmName = "Пила 8 (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });


            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://media.kg-portal.ru/movies/s/sawlegacy/posters/sawlegacy_35.jpg",
            //        FilmName = "Пила (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });

            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://i.ytimg.com/vi/aDybRqxRWc0/movieposter.jpg",
            //        FilmName = "Пила 2 (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });

            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://media.kg-portal.ru/movies/s/sawlegacy/posters/sawlegacy_35.jpg",
            //        FilmName = "Пила 3 (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });
            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://media.kg-portal.ru/movies/s/sawlegacy/posters/sawlegacy_35.jpg",
            //        FilmName = "Пила 4 (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });

            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://media.kg-portal.ru/movies/s/sawlegacy/posters/sawlegacy_35.jpg",
            //        FilmName = "Пила 5 (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });

            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://media.kg-portal.ru/movies/s/sawlegacy/posters/sawlegacy_35.jpg",
            //        FilmName = "Пила 6 (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });

            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://media.kg-portal.ru/movies/s/sawlegacy/posters/sawlegacy_35.jpg",
            //        FilmName = "Пила 7 (Jigsaw)",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });

            //    onlineCinemaContext.Movies.Add(new Movie()
            //    {
            //        Image = "https://kinogo.zone/uploads/posts/2020-04/1588032215-1304738478-pila-nachalo.jpg",
            //        FilmName = "Пила Начало",
            //        Genre = new List<Genre>(){ onlineCinemaContext.Genres.First(x => x.GenreName == "Ужасы"),
            //        onlineCinemaContext.Genres.First(x => x.GenreName == "Триллер"),
            //    onlineCinemaContext.Genres.First(x => x.GenreName == "Криминал")}
            //    });
            //}
            //onlineCinemaContext.SaveChanges();
            #endregion

        }
    }
}
