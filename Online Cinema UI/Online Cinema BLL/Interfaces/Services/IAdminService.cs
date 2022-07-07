using Microsoft.AspNetCore.Http;
using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Online_Cinema_BLL.Models.Notification;

namespace Online_Cinema_BLL.Interfaces.Services
{
    public interface IAdminService
    {
        Task<IList<string>> GetListStringGenreAsync();
        Task<IDictionary<int, string>> GetDictionaryMoviesAsync();
        Task<IDictionary<int, string>> GetDictionaryCinemaRoomsAsync();
        Task<IDictionary<int, string>> GetDictionarySessionsAsync();
        Task<Session> GetSessionAsync(int cinemaRoom);
        Task<Session> GetSessionByIdAsync(int sessionId);
        Task<IList<Session>> GetSessionsForACinemaRoomsAsync(int cinemaRoom);
        Task<IList<Session>> GetSessionsForACinemaRoomsAsync(int cinemaRoom, DateTime date);
        Task AddSessionAsync(Session session);
        Task ChangeSessionAsync(Session session);
        Task<Movie> GetMovieAsync(int movieId);
        Task AddFilmAsync(Movie movie, string genre, IFormFile file, string idUser);
        Task ChangeFilmAsync(Movie movie, string genre);
        Task<IList<CinemaRoom>> GetListCinemaRoomAsync();
        Task<CinemaRoom> GetCinemaRoomAsync(int CinemaRoomId);
        Task AddCinemaRoomAsync(CinemaRoom cinemaRoom);
        Task ChangeCinemaRoomAsync(CinemaRoom cinemaRoom);
        Task ChangeProgress(string nameFilm, int progress, string idUser, string idFilm, NotificationTypeEnum notificationType = NotificationTypeEnum.None);




        Task<IList<Room>> GetListRoomsAsync();
        Task<Room> GetRoomAsync(int roomId);
        Task<Room> GetRoomAsync(Guid userId);
    }
}