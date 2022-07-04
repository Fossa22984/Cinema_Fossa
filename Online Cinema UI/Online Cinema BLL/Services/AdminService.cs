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
using Online_Cinema_Core.UnitOfWork;
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
        private IUnitOfWork _unitOfWork;

        private readonly OnlineCinemaContext _context;


        private UploadFileAzureManager _uploadFileAzureManager;
        private FileManager _fileManager;
        private NotificationHub _notificationHub;
        private NotificationCacheManager _notificationCache;
        private SessionCacheManager _sessionCacheManager;
        private CinemaRoomCacheManager _cinemaRoomCacheManager;
        public AdminService(IUnitOfWork unitOfWork, OnlineCinemaContext context,
            UploadFileAzureManager uploadFileAzureManager,
            FileManager fileManager,
            NotificationHub notificationHub,
            NotificationCacheManager notificationCache,
            SessionCacheManager sessionCacheManager,
            CinemaRoomCacheManager cinemaRoomCacheManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _uploadFileAzureManager = uploadFileAzureManager;
            _fileManager = fileManager;
            _notificationHub = notificationHub;
            _notificationCache = notificationCache;
            _sessionCacheManager = sessionCacheManager;
            _cinemaRoomCacheManager = cinemaRoomCacheManager;
        }

        public async Task<IList<string>> GetListStringGenreAsync()
        {
            var listGenre = await _unitOfWork.Genre.GetAllGenreAsync();
            List<string> genres = new List<string>();
            foreach (var item in listGenre)
                genres.Add(item.GenreName);

            return genres.OrderBy(x => x).ToList();
        }
        public async Task<IDictionary<int, string>> GetDictionaryMoviesAsync()
        {
            var listMovies = await _unitOfWork.Movie.GetAllMovieAsync();
            var dictionary = new Dictionary<int, string>();
            foreach (var item in listMovies)
                dictionary.Add(item.Id, $"{item.MovieTitle} (#{item.Id})");

            return dictionary;
        }
        public async Task<IDictionary<int, string>> GetDictionaryCinemaRoomsAsync()
        {
            var listCinemaRooms = await _unitOfWork.CinemaRoom.GetAllCinemaRoomAsync();
            var dictionary = new Dictionary<int, string>();
            foreach (var item in listCinemaRooms)
                dictionary.Add(item.Id, $"{item.CinemaRoomName} (#{item.Id})");
            return dictionary;
        }
        public async Task<IDictionary<int, string>> GetDictionarySessionsAsync()
        {
            var listSessions = await _unitOfWork.Session.GetAllSessionAsync();
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
        public async Task<Session> GetSessionByIdAsync(int sessionId) => await _unitOfWork.Session.GetSessionByIdAsync(sessionId);
        public async Task<IList<Session>> GetSessionsForACinemaRoomsAsync(int cinemaRoom) => (await _unitOfWork.Session.GetSessionByConditionAsync(x => x.CinemaRoomId == cinemaRoom)).ToList();
        public async Task<IList<Session>> GetSessionsForACinemaRoomsAsync(int cinemaRoom, DateTime date)
        {
            var allSessions = await _unitOfWork.Session.GetSessionByConditionAsync(x => x.CinemaRoom.Id == cinemaRoom);
            return allSessions.AsEnumerable().Where(x => x.CinemaRoom.Id == cinemaRoom && x.Start.ToString("dd.MM.yyyy").Contains(date.ToString("dd.MM.yyyy"), StringComparison.OrdinalIgnoreCase)).ToList();
            //            return _context.Sessions.Include(x => x.Movie).Include(x => x.CinemaRoom).AsEnumerable()
            // .Where(x => x.CinemaRoom.Id == cinemaRoom && x.Start.ToString("dd.MM.yyyy").Contains(date.ToString("dd.MM.yyyy"), StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Start).ToList();

        }

        public async Task AddSessionAsync(Session session)
        {
            _unitOfWork.Session.CreateSession(session);
            await _unitOfWork.SaveAsync();

            _sessionCacheManager.Set(session);
            Log.Current.Debug($"Add session room id-> {session.Id} sesion id -> {session.Id}");
        }
        public async Task ChangeSessionAsync(Session session)
        {
            _unitOfWork.Session.UpdateSession(session);
            await _unitOfWork.SaveAsync();

            _sessionCacheManager.Update(session);
            Log.Current.Debug($"Change session room id-> {session.Id} sesion id -> {session.Id}");
        }

        public async Task<Movie> GetMovieAsync(int movieId) => await _unitOfWork.Movie.GetMovieByIdAsync(movieId);
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

            movie.MoviePath = moviePath;
            if (genre != null)
            {
                var res = (await _unitOfWork.Genre.GetAllGenreAsync()).AsEnumerable().Where(x => genre.Contains(x.GenreName, StringComparison.Ordinal)).ToList();
                movie.Genre = res;
            }
            await _unitOfWork.Movie.CreateMovie(movie);
            await _unitOfWork.SaveAsync();
            Log.Current.Debug($"Add movie MovieTitle -> {movie.MovieTitle} movie id-> {movie.Id}");
        }

        public async Task ChangeFilmAsync(Movie movie, string genre)
        {
            var newMovie = await _unitOfWork.Movie.GetMovieByIdAsync(movie.Id);
            if (!string.IsNullOrEmpty(genre))
            {
                var genres = (await _unitOfWork.Genre.GetAllGenreAsync()).AsEnumerable().Where(x => genre.Contains(x.GenreName, StringComparison.Ordinal)).ToList();
                newMovie.Genre = genres;
            }
            else newMovie.Genre.Clear();

            if (movie.Image.Length != 0) newMovie.Image = movie.Image;
            newMovie.Copy(movie);

            _unitOfWork.Movie.Update(newMovie);
            await _unitOfWork.SaveAsync();
            Log.Current.Debug($"Change movie MovieTitle -> {movie.MovieTitle} movie id-> {movie.Id}");
        }


        public async Task<IList<CinemaRoom>> GetListCinemaRoomAsync() => (await _unitOfWork.CinemaRoom.GetAllCinemaRoomAsync()).ToList();
        public async Task<CinemaRoom> GetCinemaRoomAsync(int CinemaRoomId) => await _unitOfWork.CinemaRoom.GetCinemaRoomByIdAsync(CinemaRoomId);
        public async Task AddCinemaRoomAsync(CinemaRoom cinemaRoom)
        {
            _unitOfWork.CinemaRoom.CreateCinemaRoom(cinemaRoom);
            await _unitOfWork.SaveAsync();

            _cinemaRoomCacheManager.Set(cinemaRoom);
            Log.Current.Debug($"Add room -> {cinemaRoom.CinemaRoomName} movie id-> {cinemaRoom.Id}");
        }
        public async Task ChangeCinemaRoomAsync(CinemaRoom cinemaRoom)
        {
            if (cinemaRoom.CinemaRoomImage.Length == 0)
                cinemaRoom.CinemaRoomImage = (await _unitOfWork.CinemaRoom.GetCinemaRoomByIdAsync(cinemaRoom.Id)).CinemaRoomImage;

            _unitOfWork.CinemaRoom.UpdateCinemaRoom(cinemaRoom);
            await _unitOfWork.SaveAsync();

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