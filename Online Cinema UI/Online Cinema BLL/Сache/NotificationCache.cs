using Online_Cinema_BLL.Extansions;
using Online_Cinema_BLL.Сache.Base;
using Online_Cinema_BLL.Сache.Models;
using OnlineCinema_Core.Config;
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