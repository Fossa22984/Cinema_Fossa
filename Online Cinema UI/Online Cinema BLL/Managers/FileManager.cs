using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http;
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
        public async Task<string> CreateTempFile(IFormFile file)
        {
            var tempFilePath = Path.GetTempFileName();
            using (var stream = File.Create(tempFilePath))
            {
                await file.CopyToAsync(stream);
            }
            return tempFilePath;
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
