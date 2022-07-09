using Online_Cinema_Domain.Models;
using Online_Cinema_Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Interfaces.Services
{
    public interface IRoomService
    {
        Task<IList<RoomViewModel>> GetListRoomsAsync();
        Task<RoomViewModel> GetRoomAsync(int roomId);
        Task<RoomViewModel> GetRoomAsync(Guid userId);
        Task<IDictionary<int, string>> GetDictionaryMoviesAsync();

        Task<MovieViewModel> GetMovieAsync(int movieId);
        Task ChangeRoomAsync(RoomViewModel room);
    }
}
