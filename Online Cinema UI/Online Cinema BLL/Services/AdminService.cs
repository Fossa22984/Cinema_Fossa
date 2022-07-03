using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Online_Cinema_BLL.Extansions;
using Online_Cinema_BLL.Managers;
using Online_Cinema_BLL.Settings;
using Online_Cinema_BLL.SignalR;
using Online_Cinema_BLL.Сache;
using Online_Cinema_BLL.Сache.Models;
using Online_Cinema_Core.Context;
using Online_Cinema_Domain.Models;
using OnlineCinema_Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services
{
    public class AdminService
    {
        private readonly OnlineCinemaContext _context;


        private UploadFileAzureManager _uploadFileAzureManager;
        private FileManager _fileManager;
        private NotificationHub _notificationHub;
        private NotificationCacheManager _notificationCache;
        private SessionCacheManager _sessionCacheManager;
        private CinemaRoomCacheManager _cinemaRoomCacheManager;
        public AdminService(OnlineCinemaContext context,
            UploadFileAzureManager uploadFileAzureManager,
            FileManager fileManager,
            NotificationHub notificationHub,
            NotificationCacheManager notificationCache,
            SessionCacheManager sessionCacheManager,
            CinemaRoomCacheManager cinemaRoomCacheManager)
        {
            this._context = context;
            _uploadFileAzureManager = uploadFileAzureManager;
            _fileManager = fileManager;
            _notificationHub = notificationHub;
            _notificationCache = notificationCache;
            _sessionCacheManager = sessionCacheManager;
            _cinemaRoomCacheManager = cinemaRoomCacheManager;
        }

        public IList<string> GetListStringGenreAsync()
        {
            var listGenre = _context.Genres.ToList();
            List<string> genres = new List<string>();
            foreach (var item in listGenre)
                genres.Add(item.GenreName);

            return genres;
        }
        public IDictionary<int, string> GetDictionaryMoviesAsync()
        {
            var listMovies = _context.Movies.ToList();
            var dictionary = new Dictionary<int, string>();
            foreach (var item in listMovies)
                dictionary.Add(item.Id, $"{item.MovieTitle} (#{item.Id})");
            return dictionary;
        }
        public IDictionary<int, string> GetDictionaryCinemaRoomsAsync()
        {
            var listCinemaRooms = _context.CinemaRooms.ToList();
            var dictionary = new Dictionary<int, string>();
            foreach (var item in listCinemaRooms)
                dictionary.Add(item.Id, $"{item.CinemaRoomName} (#{item.Id})");
            return dictionary;
        }
        public IDictionary<int, string> GetDictionarySessionsAsync()
        {
            var listSessions = _context.Sessions.Include(x => x.Movie).Include(x => x.CinemaRoom).OrderBy(x => x.Start).ToList();
            var dictionary = new Dictionary<int, string>();
            foreach (var item in listSessions)
                dictionary.Add(item.Id, $"{item.Movie.MovieTitle} ({item.Start.ToString("dd.MM.yyyy HH:mm")})");
            return dictionary;
        }


        public async Task<Session> GetSessionAsync(int cinemaRoom)
        {
            var now = DateTime.Now;
            var listSession = _sessionCacheManager.GetByCondition(x => x.CinemaRoomId == cinemaRoom)
                .Where(x => x.Start.Date == now.Date || x.Start.Date == now.AddDays(-1)).OrderBy(x => x.Start).ToList();

            return await Task.FromResult(listSession.FirstOrDefault(x => now >= x.Start && now < x.End));
        }
        public async Task<Session> GetSessionByIdAsync(int sessionId) => await _context.Sessions.Include(x => x.Movie).Where(x => x.Id == sessionId).FirstOrDefaultAsync();
        public async Task<IList<Session>> GetSessionsForACinemaRoomsAsync(int cinemaRoom) => await _context.Sessions.Include(x => x.Movie).Include(x => x.CinemaRoom).Where(x => x.CinemaRoomId == cinemaRoom).OrderBy(x => x.Start).ToListAsync();
        public async Task<IList<Session>> GetSessionsForACinemaRoomsAsync(int cinemaRoom, DateTime date) => await Task.Run(() =>
        {
            return _context.Sessions.Include(x => x.Movie).Include(x => x.CinemaRoom).AsEnumerable()
             .Where(x => x.CinemaRoom.Id == cinemaRoom && x.Start.ToString("dd.MM.yyyy").Contains(date.ToString("dd.MM.yyyy"), StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Start).ToList();
        });
        public async Task AddSessionAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            _sessionCacheManager.Set(session);
            Log.Current.Debug($"Add session room id-> {session.Id} sesion id -> {session.Id}");
        }
        public async Task ChangeSessionAsync(Session session)
        {
            var newSession = await _context.Sessions.Where(x => x.Id == session.Id).Include(x => x.Movie).Include(x => x.CinemaRoom).FirstOrDefaultAsync();
            newSession.Start = session.Start;
            newSession.End = session.End;
            newSession.MovieId = session.MovieId;
            newSession.CinemaRoomId = session.CinemaRoomId;
            await _context.SaveChangesAsync();

            _sessionCacheManager.Update(session);
            Log.Current.Debug($"Change session room id-> {session.Id} sesion id -> {session.Id}");
        }

        public async Task<Movie> GetMovieAsync(int movieId) => await _context.Movies.Include(x => x.Genre).Where(x => x.Id == movieId).FirstOrDefaultAsync();
        public async Task AddFilmAsync(Movie movie, string genre, IFormFile file, string idUser)
        {
            var idFilm = Guid.NewGuid().ToString();
            _notificationCache.Set(new Notification(idUser, idFilm, movie.MovieTitle));
            await ChangeProgress(movie.MovieTitle, 0, idUser, idFilm);

            var configPath = AppDomain.CurrentDomain.BaseDirectory;
            ConfigWrapper config = new(new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(configPath, "Settings"))
                   .AddJsonFile("azureSetings.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables() // parses the values from the optional .env file at the solution root
                   .Build());

            _fileManager.UploadProgress += ChangeProgress;
            var tempFilePath = await _fileManager.CreateTempFile(file, idFilm, idUser, movie.MovieTitle);
            movie.Duration = await _fileManager.ReadDurationFromMovie(tempFilePath);

            _uploadFileAzureManager.UploadProgress += ChangeProgress;
            var moviePath = await _uploadFileAzureManager.RunAsync(config, tempFilePath, movie.MovieTitle, idUser, idFilm);

            await _fileManager.DeleteFile(tempFilePath);
            if (genre != null)
            {
                var res = _context.Genres.AsEnumerable().Where(x => genre.Contains(x.GenreName, StringComparison.OrdinalIgnoreCase)).ToList();
                movie.Genre = res;
            }
            movie.MoviePath = moviePath;
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            Log.Current.Debug($"Add movie MovieTitle -> {movie.MovieTitle} movie id-> {movie.Id}");
        }
        public async Task ChangeFilmAsync(Movie movie, string genre)
        {
            if (genre != null)
            {
                var res = _context.Genres.AsEnumerable().Where(x => genre.Contains(x.GenreName, StringComparison.Ordinal)).ToList();
                movie.Genre = res;
            }

            var newMovie = await _context.Movies.Where(x => x.Id == movie.Id).Include(x => x.Genre).FirstOrDefaultAsync();
            if (movie.Image.Length != 0) newMovie.Image = movie.Image;
            newMovie.MovieTitle = movie.MovieTitle;
            //   newMovie.MoviePath = movie.MoviePath;
            newMovie.DateOfRelease = movie.DateOfRelease;
            newMovie.Duration = movie.Duration;
            newMovie.Author = movie.Author;
            newMovie.Actors = movie.Actors;
            newMovie.Country = movie.Country;
            newMovie.AgeLimit = movie.AgeLimit;
            newMovie.Description = movie.Description;
            newMovie.MovieBudget = movie.MovieBudget;
            newMovie.IsCartoon = movie.IsCartoon;
            newMovie.Remote = movie.Remote;
            newMovie.Genre = movie.Genre;

            await _context.SaveChangesAsync();
            Log.Current.Debug($"Change movie MovieTitle -> {movie.MovieTitle} movie id-> {movie.Id}");
        }

        public async Task<List<CinemaRoom>> GetListCinemaRoomAsync() => await _context.CinemaRooms.Include(x => x.Sessions).ThenInclude(x => x.Movie).ToListAsync();
        public async Task<CinemaRoom> GetCinemaRoomAsync(int CinemaRoomId) => await _context.CinemaRooms.Include(x => x.Sessions).ThenInclude(x => x.Movie).Where(x => x.Id == CinemaRoomId).FirstOrDefaultAsync();
        public async Task AddCinemaRoomAsync(CinemaRoom cinemaRoom)
        {
            await _context.CinemaRooms.AddAsync(cinemaRoom);
            await _context.SaveChangesAsync();

            _cinemaRoomCacheManager.Set(cinemaRoom);
            Log.Current.Debug($"Add room -> {cinemaRoom.CinemaRoomName} movie id-> {cinemaRoom.Id}");
        }
        public async Task ChangeCinemaRoomAsync(CinemaRoom cinemaRoom)
        {
            var newCinemaRoom = await _context.CinemaRooms.Where(x => x.Id == cinemaRoom.Id).Include(x => x.Sessions).FirstOrDefaultAsync();
            if (cinemaRoom.CinemaRoomImage.Length != 0) newCinemaRoom.CinemaRoomImage = cinemaRoom.CinemaRoomImage;
            newCinemaRoom.CinemaRoomName = cinemaRoom.CinemaRoomName;
            newCinemaRoom.Description = cinemaRoom.Description;
            newCinemaRoom.Remote = cinemaRoom.Remote;
            //newCinemaRoom.Sessions = cinemaRoom.Sessions;

            await _context.SaveChangesAsync();

            _cinemaRoomCacheManager.Update(cinemaRoom);
            Log.Current.Debug($"Change room -> {cinemaRoom.CinemaRoomName} movie id-> {cinemaRoom.Id}");
        }

        public async Task ChangeProgress(string nameFilm, int progress, string idUser, string idFilm)
        {
            _notificationCache.UpdateProgress(idFilm, progress);
            await _notificationHub.PushNotificationProgress(nameFilm, progress, idUser, idFilm);
        }

    }
}

//return _context.Movies.Include(x => x.Genre).AsEnumerable().Where(x => movie.Contains(x.MovieTitle, StringComparison.OrdinalIgnoreCase) && movie.Contains(x.Id.ToString(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
//return _context.CinemaRooms.Include(x => x.Sessions).AsEnumerable().Where(x => cinemaRoom.Contains(x.CinemaRoomName, StringComparison.OrdinalIgnoreCase) && cinemaRoom.Contains(x.Id.ToString(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

//public IList<string> GetListStringCinemaRooms()
//{

//    //var listCinemaRooms = _context.CinemaRooms.ToList();
//    //List<string> cinemaRooms = new List<string>();
//    //foreach (var item in listCinemaRooms)
//    //    cinemaRooms.Add($"{item.CinemaRoomName} (#{item.Id})");

//    //return cinemaRooms;
//}
