using Microsoft.AspNetCore.SignalR;
using Online_Cinema_BLL.Interfaces.SignalR;
using Online_Cinema_Domain.Models;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.SignalR
{
    public class ChatHub : Hub, IChatHub
    {
        public async Task SendMessage(string user, string message, string room, bool join)
        {
            if (join)
            {
                await JoinRoom(room).ConfigureAwait(false);
                //await Clients.Group(room).SendAsync("Send", user, " joined to " + room).ConfigureAwait(true);

            }
            else
            {
                await Clients.Group(room).SendAsync("Send", user, message).ConfigureAwait(true);
            }
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendSession(int sessionId, string room)
        {
            if (Clients != null)
                await Clients.Group(room).SendAsync("GetSession", sessionId).ConfigureAwait(true);
        }

        public async Task SendTimeCode (string room, double timeCode)
        {
            if (Clients != null)
                await Clients.Group(room).SendAsync("GetTimeCode", timeCode).ConfigureAwait(true);
        }

        public async Task SendPlay(string room)
        {
            if (Clients != null)
                await Clients.Group(room).SendAsync("GetPlay").ConfigureAwait(true);
        }

        public async Task SendPause(string room)
        {
            if (Clients != null)
                await Clients.Group(room).SendAsync("GetPause").ConfigureAwait(true);
        }

    }
}
