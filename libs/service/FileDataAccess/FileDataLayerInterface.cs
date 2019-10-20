using Tracer;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TutorBits
{
    namespace FileDataAccess
    {
        public interface FileDataLayerInterface
        {
            Task CreateFile(string path, Stream stream);

            Task<Stream> ReadFile(string path);

            Task UpdateFile(string path, Stream stream);

            Task DeleteFile(string path);

            Task<bool> FileExists(string path);

            Task<ICollection<string>> GetAllFiles(string parentPath);

            Task CreateDirectory(string path);

            Task DeleteDirectory(string path);

            Task<bool> DirectoryExists(string path);
        }
    }
}