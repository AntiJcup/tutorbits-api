using Tracer;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutorBits.Models.Common;

namespace TutorBits.FileDataAccess
{
    public interface FileDataLayerInterface
    {
        string GetWorkingDirectory();

        Task CreateFile(string path, Stream stream);

        Task<Stream> ReadFile(string path);

        Task UpdateFile(string path, Stream stream, bool append = false);

        Task DeleteFile(string path);

        Task<bool> FileExists(string path);

        Task<ICollection<string>> GetAllFiles(string parentPath);

        Task CreateDirectory(string path);

        Task DeleteDirectory(string path);

        Task<bool> DirectoryExists(string path);

        Task<string> StartMultipartUpload(string path);

        Task<string> UploadPart(string path, string multipartUploadId, int part, Stream stream, bool last);

        Task<string> StopMultipartUpload(string path, string multipartUploadId, ICollection<VideoPart> parts);

        Task CreatePathForFile(string filePath);

        Task<string> ConvertToNativePath(string path);

        Task<bool> IsDirectory(string path);
    }
}
