using Online_Cinema_BLL.Extansions;
using Online_Cinema_BLL.Сache.Base;
using Online_Cinema_Core.UnitOfWork;
using Online_Cinema_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Сache
{
    public class CinemaRoomCacheManager : BaseCacheManager<CinemaRoom>
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
