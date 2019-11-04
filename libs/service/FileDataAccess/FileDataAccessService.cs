using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Tracer;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace TutorBits
{
    namespace FileDataAccess
    {
        public class FileDataAccessService
        {
            public readonly string ProjectsDir;
            public readonly string ProjectFileName;
            public readonly string TransactionsDir;
            public readonly string TransactionLogFileName;
            public readonly string VideoDir;
            public readonly string VideoFileName;

            private readonly FileDataLayerInterface dataLayer_;

            private readonly IConfiguration configuration_;

            public FileDataAccessService(IConfiguration configuration, FileDataLayerInterface dataLayer)
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

                VideoDir = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Paths.VideoDirKey);

                VideoFileName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Paths.VideoFileNameKey);
            }

            #region Paths
            public string GetProjectPath(string projectId)
            {
                return Path.Combine(ProjectsDir, projectId);
            }

            public string GetProjectFilePath(string directory)
            {
                return Path.Combine(directory, ProjectFileName);
            }

            public string GetFullProjectFilePath(string projectId)
            {
                return GetProjectFilePath(GetProjectPath(projectId));
            }

            public string GetTransactionLogPath(string projectDirectoryPath)
            {
                return Path.Combine(projectDirectoryPath, TransactionsDir);
            }

            public string GetTransactionLogFilePath(string transactionLogPath, UInt32 partition)
            {
                return Path.Combine(transactionLogPath, string.Format(TransactionLogFileName, partition));
            }

            public string GetVideoPath(string projectId)
            {
                return Path.Combine(GetProjectPath(projectId), VideoDir);
            }

            public string GetVideoFilePath(string directory)
            {
                return Path.Combine(directory, VideoFileName);
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
            public async Task<string> AddTraceTransactionLog(Guid projectId, TraceTransactionLog transactionLog)
            {
                var project = await GetProject(projectId);
                if (project == null)
                {
                    return null;
                }

                var newProjectLength = (transactionLog.Partition + 1) * project.PartitionSize;
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

                return transactionLogFilePath;
            }

            public async Task<IDictionary<string, string>> GetTransactionLogsForRange(Guid projectId, uint offsetStart, uint offsetEnd)
            {
                var project = await GetProject(projectId);
                if (project == null)
                {
                    return null;
                }

                if (offsetStart >= project.Duration || offsetEnd > project.Duration || offsetStart >= offsetEnd)
                {
                    return null;
                }

                var projectDirectoryPath = GetProjectPath(projectId.ToString());
                var transactionLogPath = GetTransactionLogPath(projectDirectoryPath);

                var bottomPartition = project.PartitionFromOffsetBottom((int)offsetStart);
                var topPartition = project.PartitionFromOffsetTop((int)offsetEnd);
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
            #endregion

            #region Video
            public async Task<string> StartVideoRecording(string projectId)
            {
                var videoPath = GetVideoPath(projectId);
                var uploadId = await dataLayer_.StartMultipartUpload(videoPath);
                return uploadId;
            }

            public async Task ContinueVideoRecording(string projectId, string uploadId, int part, Stream videoPart)
            {
                var videoPath = GetVideoPath(projectId);
                await dataLayer_.UploadPart(videoPath, uploadId, part, videoPart);
            }

            public async Task<string> FinishVideoRecording(string projectId, string uploadId)
            {
                var videoPath = GetVideoPath(projectId);
                var videoFilePath = GetVideoFilePath(videoPath);
                return await dataLayer_.StopMultipartUpload(videoPath, uploadId, videoFilePath);
            }
            #endregion

            #region Preview
                
            #endregion
        }
    }
}