using System.Threading.Tasks;

namespace Online_Cinema_BLL.Interfaces.SignalR
{
    public interface INotificationHub
    {
        Task Subscribe(string room);
        Task PushNotificationProgress(string nameFilm, int progress, string idUser, string idFilm);
        Task GetNotifications(string idUser);
    }
}
