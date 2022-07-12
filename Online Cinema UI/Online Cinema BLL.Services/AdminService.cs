using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Online_Cinema_BLL.Interfaces.Cache;
using Online_Cinema_BLL.Interfaces.Managers;
using Online_Cinema_BLL.Interfaces.Services;
using Online_Cinema_BLL.Models;
using Online_Cinema_BLL.SignalR;
using Online_Cinema_Core.Settings.Interfaces;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models;
using Online_Cinema_Domain.Models.IdentityModels;
using Online_Cinema_Models.View;
using OnlineCinema_Core.Config;
using OnlineCinema_Core.Extensions;
using OnlineCinema_Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Online_Cinema_BLL.Models.Notification;

namespace Online_Cinema_BLL.Services
{
    public class AdminService : IAdminService
    {
        private IUnitOfWork _unitOfWork;

        private IUploadFileAzureManager _uploadFileAzureManager;
        private IFileManager _fileManager;
        private NotificationHub _notificationHub;
        private INotificationCacheManager _notificationCache;
        private ISessionCacheManager _sessionCacheManager;
        private ICinemaRoomCacheManager _cinemaRoomCacheManager;
        private IAzureSettingsManager _azureSettingsManager;
        private IMapper _mapper;
        UserManager<User> _userManager;
        public AdminService(IUnitOfWork unitOfWork,
            IUploadFileAzureManager uploadFileAzureManager,
            IFileManager fileManager,
            NotificationHub notificationHub,
            INotificationCacheManager notificationCache,
            ISessionCacheManager sessionCacheManager,
            ICinemaRoomCacheManager cinemaRoomCacheManager,
            IAzureSettingsManager azureSettingsManager,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _uploadFileAzureManager = uploadFileAzureManager;
            _fileManager = fileManager;
            _notificationHub = notificationHub;
            _notificationCache = notificationCache;
            _sessionCacheManager = sessionCacheManager;
            _cinemaRoomCacheManager = cinemaRoomCacheManager;
            _azureSettingsManager = azureSettingsManager;
            _mapper = mapper;
            _userManager = userManager;
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
            var now = DateTime.UtcNow;
            var listSession = _sessionCacheManager.GetByCondition(x => x.CinemaRoomId == cinemaRoom)
                .Where(x => x.Start.Date == now.Date || x.Start.Date == now.AddDays(-1)).OrderBy(x => x.Start).ToList();

            return await Task.FromResult(listSession.FirstOrDefault(x => now >= x.Start && now < x.End));
        }
        public async Task<SessionViewModel> GetSessionByIdAsync(int sessionId) =>
            _mapper.Map<Session, SessionViewModel>(await _unitOfWork.Session.GetSessionByIdAsync(sessionId));
        public async Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom) =>
            _mapper.Map<List<Session>, List<SessionViewModel>>((await _unitOfWork.Session.GetSessionByConditionAsync(x => x.CinemaRoomId == cinemaRoom)).ToList());
        public async Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom, DateTime date)
        {
            var allSessions = await _unitOfWork.Session.GetSessionByConditionAsync(x => x.CinemaRoom.Id == cinemaRoom);
            var sesions = allSessions.AsEnumerable().Where(x => x.CinemaRoom.Id == cinemaRoom && x.Start.ToString("dd.MM.yyyy").Contains(date.ToString("dd.MM.yyyy"), StringComparison.OrdinalIgnoreCase)).ToList();
            return _mapper.Map<List<Session>, List<SessionViewModel>>(sesions);
        }

        public async Task AddSessionAsync(SessionViewModel sessionView)
        {
            var session = _mapper.Map<SessionViewModel, Session>(sessionView);
            await _unitOfWork.Session.CreateSession(session);
            await _unitOfWork.SaveAsync();

            _sessionCacheManager.Set(session);
            Log.Current.Debug($"Add session room id-> {session.Id} sesion id -> {session.Id}");
        }
        public async Task ChangeSessionAsync(SessionViewModel sessionView)
        {
            var session = _mapper.Map<SessionViewModel, Session>(sessionView);
            await _unitOfWork.Session.UpdateSession(session);
            await _unitOfWork.SaveAsync();

            _sessionCacheManager.Update(session);
            Log.Current.Debug($"Change session room id-> {session.Id} sesion id -> {session.Id}");
        }

        public async Task<MovieViewModel> GetMovieAsync(int movieId) =>
            _mapper.Map<Movie, MovieViewModel>(await _unitOfWork.Movie.GetMovieByIdAsync(movieId));
        public async Task AddFilmAsync(MovieViewModel movieView, string genre, IFormFile file, string idUser)
        {
            string tempFilePath = string.Empty;
            try
            {
                movieView.Image = await readImageOrFillDefault(movieView.ImageFile);

                var movie = _mapper.Map<MovieViewModel, Movie>(movieView);

                var idFilm = Guid.NewGuid().ToString();
                _notificationCache.Set(new Notification(idUser, idFilm, movie.MovieTitle, notificationType: NotificationTypeEnum.StartLoad));
                await ChangeProgress(movie.MovieTitle, 0, idUser, idFilm, NotificationTypeEnum.StartLoad);

                _fileManager.UploadProgress += ChangeProgress;
                tempFilePath = await _fileManager.CreateTempFile(file, idFilm, idUser, movie.MovieTitle);
                movie.Duration = await _fileManager.ReadDurationFromMovie(tempFilePath);

                _uploadFileAzureManager.UploadProgress += ChangeProgress;
                var config = _azureSettingsManager.Get();
                var moviePath = await _uploadFileAzureManager.RunAsync(config, tempFilePath, movie.MovieTitle, idUser, idFilm);


                movie.MoviePath = moviePath;
                if (genre != null)
                {
                    var res = (await _unitOfWork.Genre.GetAllGenreAsync()).AsEnumerable().Where(x => genre.Contains(x.GenreName, StringComparison.Ordinal)).ToList();
                    movie.Genres = res;
                }
                await _unitOfWork.Movie.CreateMovie(movie);
                await _unitOfWork.SaveAsync();
                Log.Current.Debug($"Add movie MovieTitle -> {movie.MovieTitle} movie id-> {movie.Id}");
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                await _fileManager.DeleteFile(tempFilePath);
            }

        }

        public async Task ChangeFilmAsync(MovieViewModel movieView, string genre)
        {
            movieView.Image = await readImage(movieView.ImageFile);
            var movie = _mapper.Map<MovieViewModel, Movie>(movieView);

            var newMovie = await _unitOfWork.Movie.GetMovieByIdAsync(movie.Id);
            if (!string.IsNullOrEmpty(genre))
            {
                var genres = (await _unitOfWork.Genre.GetAllGenreAsync()).AsEnumerable().Where(x => genre.Contains(x.GenreName, StringComparison.Ordinal)).ToList();
                newMovie.Genres = genres;
            }
            else newMovie.Genres.Clear();

            if (movie.Image.Length != 0) newMovie.Image = movie.Image;
            newMovie.Copy(movie);

            await _unitOfWork.Movie.Update(newMovie);
            await _unitOfWork.SaveAsync();
            Log.Current.Debug($"Change movie MovieTitle -> {movie.MovieTitle} movie id-> {movie.Id}");
        }

        public async Task<CinemaRoomViewModel> GetCinemaRoomAsync(int CinemaRoomId)
        {
            return _mapper.Map<CinemaRoom, CinemaRoomViewModel>(await _unitOfWork.CinemaRoom.GetCinemaRoomByIdAsync(CinemaRoomId));
        }
        public async Task AddCinemaRoomAsync(CinemaRoomViewModel cinemaRoomView)
        {
            cinemaRoomView.CinemaRoomImage = await readImageOrFillDefault(cinemaRoomView.ImageFile);
            var cinemaRoom = _mapper.Map<CinemaRoomViewModel, CinemaRoom>(cinemaRoomView);

            await _unitOfWork.CinemaRoom.CreateCinemaRoom(cinemaRoom);
            await _unitOfWork.SaveAsync();

            _cinemaRoomCacheManager.Set(cinemaRoom);
            Log.Current.Debug($"Add room -> {cinemaRoom.CinemaRoomName} movie id-> {cinemaRoom.Id}");
        }
        public async Task ChangeCinemaRoomAsync(CinemaRoomViewModel cinemaRoomView)
        {
            cinemaRoomView.CinemaRoomImage = await readImage(cinemaRoomView.ImageFile);
            var cinemaRoom = _mapper.Map<CinemaRoomViewModel, CinemaRoom>(cinemaRoomView);

            if (cinemaRoom.CinemaRoomImage.Length == 0)
                cinemaRoom.CinemaRoomImage = (await _unitOfWork.CinemaRoom.GetCinemaRoomByIdAsync(cinemaRoom.Id)).CinemaRoomImage;

            await _unitOfWork.CinemaRoom.UpdateCinemaRoom(cinemaRoom);
            await _unitOfWork.SaveAsync();

            _cinemaRoomCacheManager.Update(cinemaRoom);
            Log.Current.Debug($"Change room -> {cinemaRoom.CinemaRoomName} movie id-> {cinemaRoom.Id}");
        }

        private async Task ChangeProgress(string nameFilm, int progress, string idUser, string idFilm, NotificationTypeEnum notificationType = NotificationTypeEnum.None)
        {
            _notificationCache.UpdateProgress(idFilm, progress);
            await _notificationHub.PushNotificationProgress(nameFilm, progress, idUser, idFilm, notificationType);
        }


        private async Task<byte[]> readImageOrFillDefault(IFormFile fromFile)
        {
            byte[] image = null;
            if (fromFile != null)
            {
                if (fromFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        fromFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        image = fileBytes;
                    }
                }
            }
            else
            {
                var fileInfo = new FileInfo(DefaultRootHelper.Current.DefaultIconPath);
                if (fileInfo.Length > 0)
                {
                    image = new byte[fileInfo.Length];
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        fs.Read(image, 0, image.Length);
                    }
                }
            }
            return await Task.FromResult(image);
        }
        private async Task<byte[]> readImage(IFormFile fromFile)
        {
            byte[] image = null;
            if (fromFile != null)
            {
                if (fromFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        fromFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        image = fileBytes;
                    }
                }
            }
            return await Task.FromResult(image);
        }
    }
}
