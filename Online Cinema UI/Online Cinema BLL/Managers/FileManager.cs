using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http;
using Online_Cinema_BLL.Сache;
using OnlineCinema_Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Cinema_BLL.Managers
{
    public class FileManager
    {
        public NotificationCache _notificationCache;

        public delegate Task ProgressChange(string nameFilm, int progress, string idUser, string idFilm);
        public event ProgressChange UploadProgress;
        public FileManager(NotificationCache notificationCache)
        {
            _notificationCache = notificationCache;
        }
        public async Task<string> CreateTempFile(IFormFile file, string idFilm, string idUser, string nameFilm)
        {
            var tempFilePath = Path.GetTempFileName();

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
            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                var duration = inputFile.Metadata.Duration;
                return await Task.FromResult(duration);
            }
        }
    }
}