using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Interfaces.Managers
{
    public interface IFileManager
    {
        public delegate Task ProgressChange(string nameFilm, int progress, string idUser, string idFilm);
        public event ProgressChange UploadProgress;
        Task<string> CreateTempFile(IFormFile file, string idFilm, string idUser, string nameFilm);
        Task DeleteFile(string path);
        Task<TimeSpan> ReadDurationFromMovie(string path);
    }
}
