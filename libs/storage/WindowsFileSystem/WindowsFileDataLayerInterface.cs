using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.FileDataAccess;

namespace TutorBits
{
    namespace WindowsFileSystem
    {
        public static class ServiceExtensions
        {
            public static IServiceCollection AddWindowsFileDataAccessLayer(this IServiceCollection services)
            {
                services.AddTransient<FileDataLayerInterface, WindowsFileDataLayerInterface>();
                return services.AddTransient<FileDataAccessService>();
            }
        }
        public class WindowsFileDataLayerInterface : FileDataLayerInterface
        {
            public async Task CreateDirectory(string path)
            {
                Directory.CreateDirectory(path);
            }

            public async Task CreateFile(string path, Stream stream)
            {
                Directory.CreateDirectory(path);
                File.Create(path);
                await UpdateFile(path, stream);
            }

            public async Task DeleteDirectory(string path)
            {
                Directory.Delete(path);
            }

            public async Task DeleteFile(string path)
            {
                File.Delete(path);
            }

            public async Task<bool> DirectoryExists(string path)
            {
                return Directory.Exists(path);
            }

            public async Task<bool> FileExists(string path)
            {
                return File.Exists(path);
            }

            public async Task<ICollection<string>> GetAllFiles(string parentPath)
            {
                return Directory.GetFiles(parentPath);
            }

            public async Task<Stream> ReadFile(string path)
            {
                using (var fileStream = File.OpenRead(path))
                {
                    var outMemoryStream = new MemoryStream();
                    await fileStream.CopyToAsync(outMemoryStream);
                    return outMemoryStream;
                }
            }

            public async Task UpdateFile(string path, Stream stream)
            {
                using (var writeStream = File.OpenWrite(path))
                {
                    await stream.CopyToAsync(writeStream);
                }
            }
        }
    }
}