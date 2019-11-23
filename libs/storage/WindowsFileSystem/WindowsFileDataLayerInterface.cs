using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;

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
            public static readonly string WorkingDirectory = Path.GetTempPath();

            public readonly string PartsSubDirectory = "parts";

            public static string RootPath(string path)
            {
                return Path.IsPathRooted(path) ? path : Path.Combine(WorkingDirectory, path);
            }

            public string GetWorkingDirectory()
            {
                return WorkingDirectory;
            }

            public async Task CreateDirectory(string path)
            {
                path = RootPath(path);
                Directory.CreateDirectory(path);
            }

            public async Task CreateFile(string path, Stream stream)
            {
                path = RootPath(path);

                using (var writeStream = File.Create(path))
                {
                    await stream.CopyToAsync(writeStream);
                }
            }

            public async Task DeleteDirectory(string path)
            {
                path = RootPath(path);
                Directory.Delete(path, true);
            }

            public async Task DeleteFile(string path)
            {
                path = RootPath(path);
                File.Delete(path);
            }

            public async Task<bool> DirectoryExists(string path)
            {
                path = RootPath(path);
                return Directory.Exists(path);
            }

            public async Task<bool> FileExists(string path)
            {
                path = RootPath(path);
                return File.Exists(path);
            }

            public async Task<ICollection<string>> GetAllFiles(string parentPath)
            {
                parentPath = RootPath(parentPath);
                return Directory.GetFiles(parentPath).Concat(Directory.GetDirectories(parentPath)).ToArray();
            }

            public async Task<Stream> ReadFile(string path)
            {
                path = RootPath(path);
                using (var fileStream = File.OpenRead(path))
                {
                    var outMemoryStream = new MemoryStream();
                    await fileStream.CopyToAsync(outMemoryStream);
                    outMemoryStream.Position = 0;
                    return outMemoryStream;
                }
            }

            public async Task<string> StartMultipartUpload(string path)
            {
                var uploadId = Guid.NewGuid().ToString();
                path = $"{RootPath(path)}_{uploadId}_{PartsSubDirectory}";

                var uploadFolderExists = await DirectoryExists(path);
                if (uploadFolderExists)
                {
                    throw new Exception($"multipart upload directory already exists {path}");
                }

                await CreateDirectory(path);

                return uploadId;
            }

            public async Task<string> UploadPart(string path, string multipartUploadId, int part, Stream stream, bool last)
            {
                path = $"{RootPath(path)}_{multipartUploadId}_{PartsSubDirectory}";

                var uploadFolderExists = await DirectoryExists(path);
                if (!uploadFolderExists)
                {
                    throw new Exception($"multipart upload directory doesnt exist {path}");
                }

                var partPath = $"{path}/{part.ToString()}";
                await CreateFile(partPath, stream);
                return "NA";
            }

            public async Task<string> StopMultipartUpload(string path, string multipartUploadId, ICollection<VideoPart> parts)
            {
                var partsPath = $"{RootPath(path)}_{multipartUploadId}_{PartsSubDirectory}";
                path = RootPath(path);

                var uploadFolderExists = await DirectoryExists(partsPath);
                if (!uploadFolderExists)
                {
                    throw new Exception($"multipart upload directory doesnt exist {partsPath}");
                }

                var allParts = await GetAllFiles(partsPath);
                foreach (var part in allParts)
                {
                    await UpdateFile(path, await ReadFile(part), true);
                }

                await DeleteDirectory(partsPath);

                return path;
            }

            public async Task UpdateFile(string path, Stream stream, bool append = false)
            {
                path = RootPath(path);
                if (!append)
                {
                    await CreateFile(path, stream);
                    return;
                }

                using (var writeStream = File.OpenWrite(path))
                {
                    writeStream.Seek(0, SeekOrigin.End);
                    await stream.CopyToAsync(writeStream);
                }
            }

            public async Task CreatePathForFile(string filePath)
            {
                filePath = RootPath(filePath);
                var parentPath = Directory.GetParent(filePath).FullName;
                await CreateDirectory(parentPath);
            }

            public async Task<string> ConvertToNativePath(string path)
            {
                return RootPath(path.Replace('/', '\\'));
            }

            public async Task<bool> IsDirectory(string path)
            {
                return await DirectoryExists(path);
            }
        }
    }
}