using Online_Cinema_BLL.Models;

namespace Online_Cinema_BLL.Interfaces.Cache
{
    public interface INotificationCacheManager : IBaseCacheManager<Notification>
    {
        void Update(Notification notification);
        void UpdateProgress(string filmId, int progress);
        void Remove(string IdFilm);
    }
}
