using Tracer;
using System;
using System.IO;
using System.Collections.Generic;

namespace TutorBits
{
    namespace FileDataAccess
    {
        public interface FileDataLayerInterface
        {
            void CreateFile(string path, Stream stream);

            Stream ReadFile(string path);

            void UpdateFile(string path, Stream stream);

            void DeleteFile(string path);

            bool FileExists(string path);

            ICollection<string> GetAllFiles(string parentPath);

            void CreateDirectory(string path);

            void DeleteDirectory(string path);

            bool DirectoryExists(string path);
        }
    }
}