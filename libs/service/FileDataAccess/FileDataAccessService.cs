using System;
using System.IO;
using Google.Protobuf;
using Tracer;

namespace TutorBits
{
    namespace FileDataAccess
    {
        public class FileDataAccessService
        {
            public readonly string ProjectsDir = "projects";

            public readonly string ProjectDirectoryFormat = "{0}/{1}";

            public readonly string ProjectFileName = "proj.tpc";

            private readonly FileDataLayerInterface dataLayer_;

            public FileDataAccessService(FileDataLayerInterface dataLayer)
            {
                dataLayer_ = dataLayer;
            }

            public void CreateTraceProject(TraceProject project)
            {
                if (!dataLayer_.DirectoryExists(ProjectsDir))
                {
                    dataLayer_.CreateDirectory(ProjectsDir);
                }
                
                var projectDirectoryPath = string.Format(ProjectDirectoryFormat, ProjectsDir, project.Id);
                var projectFilePath = Path.Combine(projectDirectoryPath, ProjectFileName);

                dataLayer_.CreateDirectory(projectDirectoryPath);
                using (var memoryStream = new MemoryStream())
                {
                    project.WriteTo(memoryStream);
                    memoryStream.Position = 0;
                    dataLayer_.CreateFile(projectFilePath, memoryStream);
                }
            }

            TraceProject GetProject(Guid id)
            {
                var projectDirectoryPath = string.Format(ProjectDirectoryFormat, ProjectsDir, id);
                var projectFilePath = Path.Combine(projectDirectoryPath, ProjectFileName);
                using (var fileStream = dataLayer_.ReadFile(projectFilePath))
                {
                    return TraceProject.Parser.ParseFrom(fileStream);
                }
            }

            void UpdateProject(TraceProject project)
            {
                var projectDirectoryPath = string.Format(ProjectDirectoryFormat, ProjectsDir, project.Id);
                var projectFilePath = Path.Combine(projectDirectoryPath, ProjectFileName);

                using (var memoryStream = new MemoryStream())
                {
                    project.WriteTo(memoryStream);
                    memoryStream.Position = 0;
                    dataLayer_.UpdateFile(projectFilePath, memoryStream);
                }
            }

            void DeleteProject(Guid id)
            {
                var projectDirectoryPath = string.Format(ProjectDirectoryFormat, ProjectsDir, id);

                dataLayer_.DeleteDirectory(projectDirectoryPath);
            }
        }
    }
}