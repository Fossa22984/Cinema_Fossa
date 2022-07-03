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
    public class SessionCacheManager : BaseCacheManager<Session>
    {
        public void Update(Session session)
        {
            lock (_bufferLocker)
            {
                var model = _models.FirstOrDefault(x => x.Id == session.Id);
                model.Copy(session);
            }
        }
    }
}
