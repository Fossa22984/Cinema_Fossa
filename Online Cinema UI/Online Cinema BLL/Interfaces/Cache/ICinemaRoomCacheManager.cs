using Online_Cinema_Domain.Models;

namespace Online_Cinema_BLL.Interfaces.Cache
{
    public interface ICinemaRoomCacheManager : IBaseCacheManager<CinemaRoom>
    {
        void Update(CinemaRoom cinemaRoom);
    }
}
