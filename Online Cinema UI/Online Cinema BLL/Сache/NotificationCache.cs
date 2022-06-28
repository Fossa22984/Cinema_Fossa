using Online_Cinema_BLL.Extansions;
using Online_Cinema_BLL.Сache.Base;
using Online_Cinema_BLL.Сache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Сache
{
    public class NotificationCache : BaseCacheManager<Notification>
    {
        public void Update(Notification notification)
        {
            lock (_bufferLocker)
            {
                var model = _models.FirstOrDefault(x => x.IdUser == notification.IdUser);
                model.Copy(notification);
            }
        }

        public void Remove(string idUser)
        {
            lock (_bufferLocker)
            {
                _models.Remove(_models.FirstOrDefault(x => x.IdUser == idUser));
            }
        }
    }
}