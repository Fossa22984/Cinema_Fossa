using Online_Cinema_BLL.Cache.Base;
using Online_Cinema_BLL.Interfaces.Cache;
using Online_Cinema_Domain.Models;
using OnlineCinema_Core.Extensions;
using System.Linq;

namespace Online_Cinema_BLL.Cache
{
    public class CinemaRoomCacheManager : BaseCacheManager<CinemaRoom>, ICinemaRoomCacheManager
    {
        public void Update(CinemaRoom cinemaRoom)
        {
            lock (_bufferLocker)
            {
                var model = _models.FirstOrDefault(x => x.Id == cinemaRoom.Id);
                model.Copy(cinemaRoom);
            }
        }
    }
}
