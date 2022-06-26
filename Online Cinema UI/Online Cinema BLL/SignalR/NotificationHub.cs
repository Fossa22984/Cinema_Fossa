using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task Subscribe(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }
        public async Task PushNotificationProgress(string nameFilm, int progress, string id)
        {
            await Clients.All.SendAsync("SendProgress", nameFilm, progress, id).ConfigureAwait(true);
        }
    }
}
