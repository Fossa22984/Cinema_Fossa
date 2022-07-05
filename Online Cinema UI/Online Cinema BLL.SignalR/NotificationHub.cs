using Microsoft.AspNetCore.SignalR;
using Online_Cinema_BLL.Interfaces.Cache;
using Online_Cinema_BLL.Interfaces.SignalR;
using System.Threading.Tasks;
using static Online_Cinema_BLL.Models.Notification;

namespace Online_Cinema_BLL.SignalR
{
    public class NotificationHub : Hub, INotificationHub
    {
        private readonly INotificationCacheManager _notificationCache;
        public NotificationHub(INotificationCacheManager notificationCache)
        {
            _notificationCache = notificationCache;
        }
        public async Task Subscribe(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }
        public async Task PushNotificationProgress(string nameFilm, int progress, string idUser, string idFilm, NotificationTypeEnum notificationType)
        {
            await Clients.Group(idUser).SendAsync("SendProgress", nameFilm, progress, idFilm, notificationType).ConfigureAwait(true);
        }

        public async Task GetNotifications(string idUser)
        {
            var notifications = _notificationCache.GetByCondition(x => x.IdUser == idUser);
            foreach (var item in notifications)
            {
                await Clients.Group(idUser).SendAsync("SendProgress", item.NameFilm, item.Progress, item.IdFilm).ConfigureAwait(true);
            }
        }
    }
}
