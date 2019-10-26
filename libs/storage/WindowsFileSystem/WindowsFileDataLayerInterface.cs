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
            public readonly string WorkingDirectory = Path.GetTempPath();

            public async Task CreateDirectory(string path)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
                Directory.CreateDirectory(path);
            }

            public async Task CreateFile(string path, Stream stream)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);

                using (var writeStream = File.Create(path))
                {
                    await stream.CopyToAsync(writeStream);
                }
            }

            public async Task DeleteDirectory(string path)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
                Directory.Delete(path, true);
            }

            public async Task DeleteFile(string path)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
                File.Delete(path);
            }

            public async Task<bool> DirectoryExists(string path)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
                return Directory.Exists(path);
            }

            public async Task<bool> FileExists(string path)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
                return File.Exists(path);
            }

            public async Task<ICollection<string>> GetAllFiles(string parentPath)
            {
                parentPath = Path.IsPathRooted(parentPath) ? parentPath : Path.Combine(WorkingDirectory, parentPath);
                return Directory.GetFiles(parentPath);
            }

            public async Task<Stream> ReadFile(string path)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
                using (var fileStream = File.OpenRead(path))
                {
                    var outMemoryStream = new MemoryStream();
                    await fileStream.CopyToAsync(outMemoryStream);
                    outMemoryStream.Position = 0;
                    return outMemoryStream;
                }
            }

            public async Task UpdateFile(string path, Stream stream)
            {
                path = Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
                using (var writeStream = File.OpenWrite(path))
                {
                    await stream.CopyToAsync(writeStream);
                }
            }
        }
    }
}