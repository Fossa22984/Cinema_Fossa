using Online_Cinema_BLL.Cache.Base;
using Online_Cinema_BLL.Interfaces.Cache;
using Online_Cinema_BLL.Models;
using OnlineCinema_Core.Config;
using OnlineCinema_Core.Extensions;
using System.Linq;

namespace Online_Cinema_BLL.Cache
{
    public class NotificationCacheManager : BaseCacheManager<Notification>, INotificationCacheManager
    {
        public void Update(Notification notification)
        {
            lock (_bufferLocker)
            {
                var model = _models.FirstOrDefault(x => x.IdFilm == notification.IdFilm);
                model.Copy(notification);
                Log.Current.Debug($"Update notiifcation model: IdUser -> {model.IdUser} IdFilm -> {model.IdFilm} NameFilm -> {model.NameFilm} Progress -> {model.Progress} from cache");
            }
        }
        public void UpdateProgress(string filmId, int progress)
        {
            var model = _models.FirstOrDefault(x => x.IdFilm == filmId);
            if (model.Progress != progress)
            {
                model.Progress = progress;
                Log.Current.Debug($"Update progress of notiifcation model: IdUser -> {model.IdUser} IdFilm -> {model.IdFilm} NameFilm -> {model.NameFilm} Progress -> {model.Progress} from cache");
            }
        }

        public void Remove(string IdFilm)
        {
            lock (_bufferLocker)
            {
                _models.Remove(_models.FirstOrDefault(x => x.IdFilm == IdFilm));
                Log.Current.Debug($"Remove notiifcation model: IdFilm -> {IdFilm}");
            }
        }
    }
}
