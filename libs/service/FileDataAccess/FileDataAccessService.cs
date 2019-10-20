using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Tracer;

namespace TutorBits
{
    namespace FileDataAccess
    {
        public class FileDataAccessService
        {
            public readonly string ProjectsDir = "projects";

            public readonly string ProjectFileName = "proj.tpc";

            private readonly FileDataLayerInterface dataLayer_;

            public FileDataAccessService(FileDataLayerInterface dataLayer)
            {
                dataLayer_ = dataLayer;
            }

            public string GetProjectPath(string id)
            {
                return Path.Combine(ProjectsDir, id);
            }

            public async Task CreateTraceProject(TraceProject project)
            {
                var projDirectoryExists = await dataLayer_.DirectoryExists(ProjectsDir);
                if (!projDirectoryExists)
                {
                    await dataLayer_.CreateDirectory(ProjectsDir);
                }

                var projectDirectoryPath = GetProjectPath(project.Id);
                var projectFilePath = Path.Combine(projectDirectoryPath, ProjectFileName);

                await dataLayer_.CreateDirectory(projectDirectoryPath);
                using (var memoryStream = new MemoryStream())
                {
                    project.WriteTo(memoryStream);
                    memoryStream.Position = 0;
                    await dataLayer_.CreateFile(projectFilePath, memoryStream);
                }
            }

            public async Task<TraceProject> GetProject(Guid id)
            {
                var projectDirectoryPath = GetProjectPath(id.ToString());
                var projectFilePath = Path.Combine(projectDirectoryPath, ProjectFileName);
                using (var fileStream = await dataLayer_.ReadFile(projectFilePath))
                {
                    return TraceProject.Parser.ParseFrom(fileStream);
                }
            }

            public async Task UpdateProject(TraceProject project)
            {
                var projectDirectoryPath = GetProjectPath(project.Id);
                var projectFilePath = Path.Combine(projectDirectoryPath, ProjectFileName);

                using (var memoryStream = new MemoryStream())
                {
                    project.WriteTo(memoryStream);
                    memoryStream.Position = 0;
                    await dataLayer_.UpdateFile(projectFilePath, memoryStream);
                }
            }

            public async Task DeleteProject(Guid id)
            {
                var projectDirectoryPath = GetProjectPath(id.ToString());

                await dataLayer_.DeleteDirectory(projectDirectoryPath);
            }
        }
    }
}