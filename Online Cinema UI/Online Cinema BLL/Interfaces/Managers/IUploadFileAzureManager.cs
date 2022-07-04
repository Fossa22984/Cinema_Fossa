using Online_Cinema_BLL.Settings;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Interfaces.Managers
{
    public interface IUploadFileAzureManager
    {
        delegate Task ProgressChange(string nameFilm, int progress, string idUser, string idFilm);
        public event ProgressChange UploadProgress;
        Task<string> RunAsync(ConfigWrapper config, string filePath, string name, string idUser, string idFilm);
    }
}
