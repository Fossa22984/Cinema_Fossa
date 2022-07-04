using Online_Cinema_Domain.Models;

namespace Online_Cinema_BLL.Interfaces.Cache
{
    public interface ISessionCacheManager : IBaseCacheManager<Session>
    {
        void Update(Session session);
    }
}
