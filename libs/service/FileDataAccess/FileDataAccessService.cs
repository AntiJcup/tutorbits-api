using System;
using System.Collections.Generic;
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

            public readonly string TransactionsDir = "partitions";

            public readonly string TransactionLogFileName = "{0}.tlc";

            private readonly FileDataLayerInterface dataLayer_;

            public FileDataAccessService(FileDataLayerInterface dataLayer)
            {
                dataLayer_ = dataLayer;
            }

            #region Paths
            public string GetProjectPath(string id)
            {
                return Path.Combine(ProjectsDir, id);
            }

            public string GetProjectFilePath(string directory)
            {
                return Path.Combine(directory, ProjectFileName);
            }

            public string GetTransactionLogPath(string projectDirectoryPath)
            {
                return Path.Combine(projectDirectoryPath, TransactionsDir);
            }

            public string GetTransactionLogFilePath(string transactionLogPath, UInt32 partition)
            {
                return Path.Combine(transactionLogPath, string.Format(TransactionLogFileName, partition));
            }
            #endregion



            #region Project
            public async Task CreateTraceProject(TraceProject project)
            {
                var projDirectoryExists = await dataLayer_.DirectoryExists(ProjectsDir);
                if (!projDirectoryExists)
                {
                    await dataLayer_.CreateDirectory(ProjectsDir);
                }

                var projectDirectoryPath = GetProjectPath(project.Id);
                var projectFilePath = GetProjectFilePath(projectDirectoryPath);

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

            public async Task DeleteProject(Guid id)
            {
                var projectDirectoryPath = GetProjectPath(id.ToString());

                await dataLayer_.DeleteDirectory(projectDirectoryPath);
            }
            #endregion

            #region TransactionLog
            public async Task AddTraceTransactionLog(Guid projectId, TraceTransactionLog transactionLog)
            {
                var project = await GetProject(projectId);
                if (project == null)
                {
                    return;
                }

                var newProjectLength = transactionLog.Partition * project.PartitionSize;
                if (project.Duration < newProjectLength)
                {
                    project.Duration = newProjectLength;
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
            }

            public async Task<ICollection<string>> GetTransactionLogsForRange(Guid projectId, uint offsetStart, uint offsetEnd)
            {
                var project = await GetProject(projectId);
                if (project == null)
                {
                    return null;
                }

                if (offsetStart > project.Duration || offsetEnd > project.Duration || offsetStart >= offsetEnd)
                {
                    return null;
                }

                var projectDirectoryPath = GetProjectPath(projectId.ToString());
                var transactionLogPath = GetTransactionLogPath(projectDirectoryPath);
                return await dataLayer_.GetAllFiles(transactionLogPath);
            }
            #endregion
        }
    }
}