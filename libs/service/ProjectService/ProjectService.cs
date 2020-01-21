using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tracer;
using Google.Protobuf;
using TutorBits.FileDataAccess;
using System.Collections.Generic;
using System.Linq;

namespace TutorBits.Project
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddProjectService(this IServiceCollection services)
        {
            return services.AddTransient<ProjectService>();
        }
    }

    public class ProjectService
    {
        private readonly FileDataLayerInterface dataLayer_;

        private readonly IConfiguration configuration_;

        public readonly string ProjectsDir;
        public readonly string ProjectFileName;
        public readonly string TransactionsDir;
        public readonly string TransactionLogFileName;
        public readonly string ProjectResourceDir;
        public readonly string ProjectResourceFileName;

        public ProjectService(IConfiguration configuration, FileDataLayerInterface dataLayer)
        {
            configuration_ = configuration;
            dataLayer_ = dataLayer;

            ProjectsDir = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectsDirKey);

            ProjectFileName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectFileNameKey);

            TransactionsDir = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.TransactionsDirKey);

            TransactionLogFileName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.TransactionLogFileNameKey);

            ProjectResourceDir = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectResourceDirKey);

            ProjectResourceFileName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectResourceFileNameKey);
        }

        public string GetProjectPath(string projectId)
        {
            var workingDirectory = dataLayer_.GetWorkingDirectory();
            return dataLayer_.SanitizePath(string.IsNullOrWhiteSpace(workingDirectory) ? Path.Combine(ProjectsDir, projectId) : Path.Combine(workingDirectory, ProjectsDir, projectId));
        }

        public string GetProjectFilePath(string directory)
        {
            return dataLayer_.SanitizePath(Path.Combine(directory, ProjectFileName));
        }

        public string GetFullProjectFilePath(string projectId)
        {
            return dataLayer_.SanitizePath(GetProjectFilePath(GetProjectPath(projectId)));
        }

        public string GetTransactionLogPath(string projectDirectoryPath)
        {
            return dataLayer_.SanitizePath(Path.Combine(projectDirectoryPath, TransactionsDir));
        }

        public string GetTransactionLogFilePath(string transactionLogPath, UInt32 partition)
        {
            return dataLayer_.SanitizePath(Path.Combine(transactionLogPath, string.Format(TransactionLogFileName, partition)));
        }

        public string GetProjectResourceDir(string directory)
        {
            return dataLayer_.SanitizePath(Path.Combine(directory, ProjectResourceDir));
        }

        public string GetProjectResourceFilePath(string resourceDirectory, string resourceId)
        {
            return dataLayer_.SanitizePath(Path.Combine(resourceDirectory, string.Format(ProjectResourceFileName, resourceId)));
        }

        public async Task CreateTraceProject(TraceProject project)
        {
            var projectDirectoryPath = GetProjectPath(project.Id);
            var projectFilePath = GetProjectFilePath(projectDirectoryPath);

            var projExists = await dataLayer_.FileExists(projectFilePath);
            if (projExists)
            {
                throw new Exception("Project already exists");
            }

            await dataLayer_.CreateDirectory(projectDirectoryPath);
            using (var memoryStream = new MemoryStream())
            {
                project.WriteTo(memoryStream);
                memoryStream.Position = 0;
                await dataLayer_.CreateFile(projectFilePath, memoryStream);
            }
        }

        public async Task<bool> DoesProjectExist(Guid id)
        {
            var projectDirectoryPath = GetProjectPath(id.ToString());
            return await dataLayer_.DirectoryExists(projectDirectoryPath);
        }

        public async Task<TraceProject> GetProject(Guid id)
        {
            var projectDirectoryPath = GetProjectPath(id.ToString());
            var projectFilePath = GetProjectFilePath(projectDirectoryPath);
            using (var fileStream = await dataLayer_.ReadFile(projectFilePath))
            {
                var project = TraceProject.Parser.ParseFrom(fileStream);
                return project.CalculateSize() > 0 ? project : null;
            }
        }

        public async Task UpdateProject(TraceProject project)
        {
            var projectDirectoryPath = GetProjectPath(project.Id);
            var projectFilePath = GetProjectFilePath(projectDirectoryPath);

            using (var memoryStream = new MemoryStream())
            {
                project.WriteTo(memoryStream);
                memoryStream.Position = 0;
                await dataLayer_.UpdateFile(projectFilePath, memoryStream);
            }
        }

        public async Task ResetProject(Guid id)
        {
            var projectDirectoryPath = GetProjectPath(id.ToString());

            if ((await dataLayer_.DirectoryExists(projectDirectoryPath)))
            {
                await dataLayer_.DeleteDirectory(projectDirectoryPath);
            }
        }

        public async Task<string> AddResource(Guid projectId, Stream resourceStream, Guid resourceId)
        {
            var projectDirectoryPath = GetProjectPath(projectId.ToString());
            var projectResourcePath = GetProjectResourceDir(projectDirectoryPath);
            var projectResourceFilePath = GetProjectResourceFilePath(projectResourcePath, resourceId.ToString());

            var projectResourcePathExists = await dataLayer_.DirectoryExists(projectResourcePath);
            if (!projectResourcePathExists)
            {
                await dataLayer_.CreateDirectory(projectResourcePath);
            }

            await dataLayer_.CreateFile(projectResourceFilePath, resourceStream);

            return projectResourceFilePath;
        }

        public async Task<string> AddTraceTransactionLog(Guid projectId, TraceTransactionLog transactionLog)
        {
            var project = await GetProject(projectId);
            if (project == null)
            {
                return null;
            }

            var latestTransaction = transactionLog.Transactions.OrderByDescending(t => t.TimeOffsetMs).FirstOrDefault();
            if (latestTransaction != null && project.Duration < latestTransaction.TimeOffsetMs)
            {
                project.Duration = latestTransaction.TimeOffsetMs;
                await UpdateProject(project);
            }

            var projectDirectoryPath = GetProjectPath(projectId.ToString());
            var transactionLogPath = GetTransactionLogPath(projectDirectoryPath);
            var transactionLogFilePath = GetTransactionLogFilePath(transactionLogPath, transactionLog.Partition);

            var transactionLogPathExists = await dataLayer_.DirectoryExists(transactionLogPath);
            if (!transactionLogPathExists)
            {
                await dataLayer_.CreateDirectory(transactionLogPath);
            }

            using (var memoryStream = new MemoryStream())
            {
                transactionLog.WriteTo(memoryStream);
                memoryStream.Position = 0;
                await dataLayer_.CreateFile(transactionLogFilePath, memoryStream);
            }

            return transactionLogFilePath;
        }

        public async Task<IDictionary<string, string>> GetTransactionLogsForRange(Guid projectId, uint offsetStart, uint offsetEnd)
        {
            var project = await GetProject(projectId);
            if (project == null)
            {
                return null;
            }


            var projectDirectoryPath = GetProjectPath(projectId.ToString());
            var transactionLogPath = GetTransactionLogPath(projectDirectoryPath);

            var bottomPartition = project.PartitionFromOffsetBottom((int)offsetStart);
            var topPartition = project.PartitionFromOffsetTop((int)offsetEnd) + 1;
            List<string> partitionRange = new List<string>();
            for (var i = bottomPartition; i < topPartition; i++)
            {
                partitionRange.Add(i.ToString());
            }

            var partitionFiles = await dataLayer_.GetAllFiles(transactionLogPath);

            var partitionDictionary = new Dictionary<string, string>();
            foreach (var partitionFile in partitionFiles)
            {
                var partitionNumber = Path.GetFileName(partitionFile).Split('.')[0];
                if (!partitionRange.Contains(partitionNumber))
                {
                    continue;
                }
                partitionDictionary[partitionNumber] = partitionFile;
            }

            return partitionDictionary;
        }
    }
}

