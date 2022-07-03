using Microsoft.AspNetCore.SignalR;
using Online_Cinema_BLL.Сache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.SignalR
{
    public class NotificationHub : Hub
    {
        public NotificationCacheManager _notificationCache { get; set; }
        public NotificationHub(NotificationCacheManager notificationCache)
        {
            _notificationCache = notificationCache;
        }
        public async Task Subscribe(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }
        public async Task PushNotificationProgress(string nameFilm, int progress, string idUser, string idFilm)
        {
            await Clients.Group(idUser).SendAsync("SendProgress", nameFilm, progress, idFilm).ConfigureAwait(true);
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
