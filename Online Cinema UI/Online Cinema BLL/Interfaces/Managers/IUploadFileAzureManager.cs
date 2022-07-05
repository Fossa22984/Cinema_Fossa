using Online_Cinema_Core.Settings.Models;
using System.Threading.Tasks;
using static Online_Cinema_BLL.Models.Notification;

namespace Online_Cinema_BLL.Interfaces.Managers
{
    public interface IUploadFileAzureManager
    {
        delegate Task ProgressChange(string nameFilm, int progress, string idUser, string idFilm, NotificationTypeEnum notificationType = NotificationTypeEnum.None);
        public event ProgressChange UploadProgress;
        Task<string> RunAsync(AzureSettingsModel config, string filePath, string name, string idUser, string idFilm);
    }
}
