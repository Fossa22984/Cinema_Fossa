using Microsoft.AspNetCore.Http;
using Online_Cinema_Domain.Models;
using Online_Cinema_Models.View;
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
        Task<SessionViewModel> GetSessionByIdAsync(int sessionId);
        Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom);
        Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom, DateTime date);
        Task AddSessionAsync(SessionViewModel session);
        Task ChangeSessionAsync(SessionViewModel session);
        Task<MovieViewModel> GetMovieAsync(int movieId);
        Task AddFilmAsync(MovieViewModel movie, string genre, IFormFile file, string idUser);
        Task ChangeFilmAsync(MovieViewModel movie, string genre);
        Task<CinemaRoomViewModel> GetCinemaRoomAsync(int CinemaRoomId);
        Task AddCinemaRoomAsync(CinemaRoomViewModel cinemaRoom);
        Task ChangeCinemaRoomAsync(CinemaRoomViewModel cinemaRoom);

    }
}