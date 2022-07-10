using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Online_Cinema_BLL.Interfaces.Cache;
using Online_Cinema_BLL.Interfaces.Managers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Services.Managers
{
    public class FileManager : IFileManager
    {
        public INotificationCacheManager _notificationCache;

        public delegate Task ProgressChange(string nameFilm, int progress, string idUser, string idFilm);
        public event IFileManager.ProgressChange UploadProgress;

        private IHostingEnvironment _environment;
        public FileManager(INotificationCacheManager notificationCache, IHostingEnvironment environment)
        {
            _notificationCache = notificationCache;

            _environment = environment;
        }
        public async Task<string> CreateTempFile(IFormFile file, string idFilm, string idUser, string nameFilm)
        {
            //var tempFilePath = Path.GetTempFileName();


            var filename = Guid.NewGuid().ToString();
            var root = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

            var tempFilePath = Path.Combine(root, "Temp", filename);
            if (!Directory.Exists(Path.Combine(root, "Temp")))
            {
                Directory.CreateDirectory(Path.Combine(root, "Temp"));
            }


            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            bool cancelFlag = false;

            using (Stream source = file.OpenReadStream())
            {
                long fileLength = source.Length;
                using (FileStream dest = File.Create(tempFilePath))
                {
                    long totalBytes = 0;
                    int currentBlockSize = 0;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytes += currentBlockSize;
                        double percentage = (double)totalBytes * 100.0 / fileLength;

                        dest.Write(buffer, 0, currentBlockSize);
                        var percentageInt = (int)percentage;
                        if (percentageInt % 5 == 0)
                            await UploadProgress?.Invoke(nameFilm, percentageInt / 2, idUser, idFilm);
                        //_notificationCache.UpdateProgress(idFilm, percentageInt / 2);

                        if (cancelFlag == true || percentage == 100)
                        {
                            // Delete dest file here
                            break;
                        }
                    }
                }
            }

            return await Task.FromResult(tempFilePath);
        }

        public async Task DeleteFile(string path)
        {
            File.Delete(path);
            await Task.CompletedTask;
        }

        public async Task<TimeSpan> ReadDurationFromMovie(string path)
        {
            var inputFile = new MediaFile(path);
            string pathEngine = Path.Combine(_environment.WebRootPath, "~/wwwroot/ffmpeg.exe");
            using (var engine = new Engine(pathEngine))
            {

                engine.GetMetadata(inputFile);
                var duration = inputFile.Metadata.Duration;
                return await Task.FromResult(duration);
            }
        }
    }
}
