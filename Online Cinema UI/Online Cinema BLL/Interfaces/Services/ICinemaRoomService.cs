using Online_Cinema_Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Interfaces.Services
{
    public interface ICinemaRoomService
    {
        Task<IList<CinemaRoomViewModel>> GetListCinemaRoomAsync();
        Task<CinemaRoomViewModel> GetCinemaRoomAsync(int CinemaRoomId);
        Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom, DateTime date);
        Task<SessionViewModel> GetSessionByIdAsync(int sessionId);
        Task<IList<SessionViewModel>> GetSessionsForACinemaRoomsAsync(int cinemaRoom);
    }
}
