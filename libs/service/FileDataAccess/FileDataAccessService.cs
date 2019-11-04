using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Tracer;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text;

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
            public readonly string PreviewsDir;

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

                PreviewsDir = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectsDirKey);
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

            public string GetPreviewPath(string projectId, string previewId)
            {
                return Path.Combine(PreviewsDir, projectId, previewId);
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
            public async Task<string> StartVideoRecording(Guid projectId)
            {
                var videoPath = GetVideoPath(projectId.ToString());
                var uploadId = await dataLayer_.StartMultipartUpload(videoPath);
                return uploadId;
            }

            public async Task ContinueVideoRecording(Guid projectId, string uploadId, int part, Stream videoPart)
            {
                var videoPath = GetVideoPath(projectId.ToString());
                await dataLayer_.UploadPart(videoPath, uploadId, part, videoPart);
            }

            public async Task<string> FinishVideoRecording(Guid projectId, string uploadId)
            {
                var videoPath = GetVideoPath(projectId.ToString());
                var videoFilePath = GetVideoFilePath(videoPath);
                return await dataLayer_.StopMultipartUpload(videoPath, uploadId, videoFilePath);
            }
            #endregion

            #region Preview
            public async Task<string> GeneratePreview(TraceProject project, int end)
            {
                var previewDirectoryExists = await dataLayer_.DirectoryExists(PreviewsDir);
                if (!previewDirectoryExists)
                {
                    await dataLayer_.CreateDirectory(PreviewsDir);
                }

                var projectId = Guid.Parse(project.Id);
                var previewId = Guid.NewGuid().ToString();
                var previewPath = GetPreviewPath(project.Id, previewId);
                var transactionLogPaths = await GetTransactionLogsForRange(projectId, 0, (uint)end);
                var files = new Dictionary<string, StringBuilder>();
                foreach (var transactionLogPath in transactionLogPaths)
                {
                    using (var transactionLogStream = await dataLayer_.ReadFile(transactionLogPath.Value))
                    {
                        var transactionLog = TraceTransactionLog.Parser.ParseFrom(transactionLogStream);
                        foreach (var transaction in transactionLog.Transactions)
                        {
                            if (transaction.TimeOffsetMs > end)
                            {
                                continue;
                            }

                            StringBuilder file = null;
                            if (files.ContainsKey(transaction.FilePath))
                            {
                                file = files[transaction.FilePath];
                            }
                            else
                            {
                                file = new StringBuilder();
                                files[transaction.FilePath] = file;
                            }

                            switch (transaction.Type)
                            {
                                case TraceTransaction.Types.TraceTransactionType.CreateFile:
                                    break;
                                case TraceTransaction.Types.TraceTransactionType.DeleteFile:
                                    files.Remove(transaction.FilePath);
                                    break;
                                case TraceTransaction.Types.TraceTransactionType.RenameFile:
                                    var renameFileData = transaction.RenameFile;
                                    files[renameFileData.NewFilePath] = file;
                                    files.Remove(transaction.FilePath);
                                    break;
                                case TraceTransaction.Types.TraceTransactionType.ModifyFile:
                                    var modifyFileData = transaction.ModifyFile;
                                    if (modifyFileData.OffsetStart < file.Length && (modifyFileData.OffsetEnd != modifyFileData.OffsetStart))
                                    {
                                        file.Remove((int)modifyFileData.OffsetStart, (int)modifyFileData.OffsetEnd);
                                    }

                                    if (!string.IsNullOrEmpty(modifyFileData.Data))
                                    {
                                        file.Insert((int)modifyFileData.OffsetStart, modifyFileData.Data);
                                    }
                                    break;
                            }
                        }
                    }
                }

                foreach (var file in files)
                {
                    var path = file.Key;
                    await dataLayer_.CreatePathForFile(path);
                    var fileBytes = Encoding.UTF8.GetBytes(file.ToString());
                    using (var stream = new MemoryStream(fileBytes))
                    {
                        await dataLayer_.CreateFile(path, stream);
                    }
                }

                return previewPath;
            }
            #endregion
        }
    }
}