using System.Threading.Tasks;
using static Online_Cinema_BLL.Models.Notification;

namespace Online_Cinema_BLL.Interfaces.SignalR
{
    public interface INotificationHub
    {
        Task Subscribe(string room);
        Task PushNotificationProgress(string nameFilm, int progress, string idUser, string idFilm, NotificationTypeEnum notificationType);
        Task GetNotifications(string idUser);
    }
}
