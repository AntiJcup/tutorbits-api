using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Tracer;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text;
using TutorBits.Models.Common;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TutorBits.FileDataAccess
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
        public readonly string ThumbnailFileName;
        public readonly string ProjectZipName;
        public readonly string ProjectJsonName;

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
                .GetValue<string>(Constants.Configuration.Sections.Paths.PreviewsDirKey);

            ThumbnailFileName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ThumbnailFileNameKey);

            ProjectZipName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectZipNameKey);

            ProjectJsonName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectJsonNameKey);
        }

        #region Paths
        public string SanitizePath(string path)
        {
            return path.Replace("\\", "/");
        }

        public string GetWorkingDirectory()
        {
            return SanitizePath(dataLayer_.GetWorkingDirectory());
        }

        public string GetProjectPath(string projectId)
        {
            var workingDirectory = GetWorkingDirectory();
            return SanitizePath(string.IsNullOrWhiteSpace(workingDirectory) ? Path.Combine(ProjectsDir, projectId) : Path.Combine(workingDirectory, ProjectsDir, projectId));
        }

        public string GetProjectFilePath(string directory)
        {
            return SanitizePath(Path.Combine(directory, ProjectFileName));
        }

        public string GetFullProjectFilePath(string projectId)
        {
            return SanitizePath(GetProjectFilePath(GetProjectPath(projectId)));
        }

        public string GetTransactionLogPath(string projectDirectoryPath)
        {
            return SanitizePath(Path.Combine(projectDirectoryPath, TransactionsDir));
        }

        public string GetTransactionLogFilePath(string transactionLogPath, UInt32 partition)
        {
            return SanitizePath(Path.Combine(transactionLogPath, string.Format(TransactionLogFileName, partition)));
        }

        public string GetVideoPath(string projectId)
        {
            return SanitizePath(Path.Combine(GetProjectPath(projectId), VideoDir));
        }

        public string GetVideoFilePath(string directory)
        {
            return SanitizePath(Path.Combine(directory, VideoFileName));
        }

        public string GetPreviewPath(string projectId, string previewId)
        {
            return SanitizePath(Path.Combine(PreviewsDir, projectId, previewId));
        }

        public string GetThumbnailFilePath(string directory)
        {
            return SanitizePath(Path.Combine(directory, ThumbnailFileName));
        }

        public string GetProjectZipFilePath(string directory)
        {
            return SanitizePath(Path.Combine(directory, ProjectZipName));
        }

        public string GetProjectJsonFilePath(string directory)
        {
            return SanitizePath(Path.Combine(directory, ProjectJsonName));
        }
        #endregion

        #region Project
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
            var transactionLogPath = GetTransactionLogPath(projectDirectoryPath);
            var projectFilePath = GetProjectFilePath(projectDirectoryPath);
            var videoPath = GetVideoPath(id.ToString());

            if ((await dataLayer_.DirectoryExists(transactionLogPath)))
            {
                await dataLayer_.DeleteDirectory(transactionLogPath);
            }

            if ((await dataLayer_.DirectoryExists(videoPath)))
            {
                await dataLayer_.DeleteDirectory(videoPath);
            }

            if ((await dataLayer_.FileExists(projectFilePath)))
            {
                await dataLayer_.DeleteFile(projectFilePath);
            }
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
        #endregion

        #region Video
        public async Task<string> StartVideoRecording(Guid projectId)
        {
            var videoPath = GetVideoPath(projectId.ToString());
            var videoFilePath = GetVideoFilePath(videoPath);
            var uploadId = await dataLayer_.StartMultipartUpload(videoFilePath);
            return uploadId;
        }

        public async Task<string> ContinueVideoRecording(Guid projectId, string uploadId, int part, Stream videoPart, bool last)
        {
            var videoPath = GetVideoPath(projectId.ToString());
            var videoFilePath = GetVideoFilePath(videoPath);
            return await dataLayer_.UploadPart(videoFilePath, uploadId, part, videoPart, last);
        }

        public async Task<string> FinishVideoRecording(Guid projectId, string uploadId, ICollection<VideoPart> parts)
        {
            var videoPath = GetVideoPath(projectId.ToString());
            var videoFilePath = GetVideoFilePath(videoPath);
            return await dataLayer_.StopMultipartUpload(videoFilePath, uploadId, parts);
        }
        #endregion

        #region Preview
        public void GeneratePreviewForTransactionLog(TraceTransactionLog transactionLog, int end, Dictionary<string, PreviewItem> files)
        {
            foreach (var transaction in transactionLog.Transactions)
            {
                if (transaction.TimeOffsetMs > end || string.IsNullOrWhiteSpace(transaction.FilePath))
                {
                    continue;
                }

                PreviewItem file = null;
                var filePath = transaction.FilePath;
                if (files.ContainsKey(filePath))
                {
                    file = files[filePath];
                }
                else
                {
                    file = new PreviewItem
                    {
                        stringBuilder = new StringBuilder(),
                        isFolder = false
                    };
                }
                files[filePath] = file;

                switch (transaction.Type)
                {
                    case TraceTransaction.Types.TraceTransactionType.CreateFile:
                        var createFileData = transaction.CreateFile;
                        files[createFileData.NewFilePath] = new PreviewItem
                        {
                            stringBuilder = new StringBuilder(),
                            isFolder = createFileData.IsFolder
                        };
                        break;
                    case TraceTransaction.Types.TraceTransactionType.DeleteFile:
                        files.Remove(filePath);
                        var deleteFileData = transaction.RenameFile;
                        file.isFolder = deleteFileData.IsFolder;
                        break;
                    case TraceTransaction.Types.TraceTransactionType.RenameFile:
                        var renameFileData = transaction.RenameFile;
                        files[renameFileData.NewFilePath] = file;
                        files.Remove(filePath);
                        file.isFolder = renameFileData.IsFolder;
                        break;
                    case TraceTransaction.Types.TraceTransactionType.ModifyFile:
                        file.isFolder = false;
                        var modifyFileData = transaction.ModifyFile;
                        if (modifyFileData.OffsetStart < file.stringBuilder.Length &&
                        (modifyFileData.OffsetEnd != modifyFileData.OffsetStart))
                        {
                            var length = (int)(modifyFileData.OffsetEnd - modifyFileData.OffsetStart);
                            file.stringBuilder = file.stringBuilder.Remove((int)modifyFileData.OffsetStart, length);
                        }

                        if (!string.IsNullOrEmpty(modifyFileData.Data))
                        {
                            file.stringBuilder = file.stringBuilder.Insert((int)modifyFileData.OffsetStart, modifyFileData.Data);
                        }
                        break;
                }
            }
        }

        public async Task SavePreviewCache(Dictionary<string, PreviewItem> files, string previewFolder)
        {
            foreach (var file in files)
            {
                var path = (await dataLayer_.ConvertToNativePath(file.Key)).Substring(1);
                var fullPath = SanitizePath(Path.Combine(previewFolder, path));
                await dataLayer_.CreatePathForFile(fullPath);
                var fileBytes = Encoding.UTF8.GetBytes(file.Value.stringBuilder.ToString());
                using (var stream = new MemoryStream(fileBytes))
                {
                    if (file.Value.isFolder)
                    {
                        await dataLayer_.CreateDirectory(fullPath);
                    }
                    else
                    {
                        await dataLayer_.CreateFile(fullPath, stream);
                    }
                }
            }
        }

        public async Task<Dictionary<string, PreviewItem>> GeneratePreview(TraceProject project, int end, string previewId)
        {
            var projectId = Guid.Parse(project.Id);
            var previewPath = GetPreviewPath(project.Id, previewId);

            var transactionLogPaths = await GetTransactionLogsForRange(projectId, 0, (uint)end);
            var files = new Dictionary<string, PreviewItem>();
            foreach (var transactionLogPath in transactionLogPaths.OrderBy(p => int.Parse(p.Key)))
            {
                using (var transactionLogStream = await dataLayer_.ReadFile(transactionLogPath.Value))
                {
                    var transactionLog = TraceTransactionLog.Parser.ParseFrom(transactionLogStream);
                    GeneratePreviewForTransactionLog(transactionLog, end, files);
                }
            }

            await SavePreviewCache(files, previewPath);

            return files;
        }

        public async Task GeneratePreview(TraceProject project, int end, string previewId, TraceTransactionLogs traceTransactionLogs)
        {
            var projectId = Guid.Parse(project.Id);
            var previewPath = GetPreviewPath(project.Id, previewId);
            var files = new Dictionary<string, PreviewItem>();
            foreach (var transactionLog in traceTransactionLogs.Logs)
            {
                GeneratePreviewForTransactionLog(transactionLog, end, files);
            }

            await SavePreviewCache(files, previewPath);
        }

        public async Task<ICollection<string>> GetAllFilesRecursive(string parentPath)
        {
            var result = new List<string>();
            var files = await dataLayer_.GetAllFiles(parentPath);
            foreach (var file in files)
            {
                var isDirectory = await dataLayer_.IsDirectory(file);
                if (isDirectory)
                {
                    var childFiles = await GetAllFilesRecursive(file);
                    result = result.Concat(childFiles).ToList();
                }

                result.Add(file);
            }

            return result;
        }

        public async Task PackagePreviewZIP(Guid projectId, string previewId)
        {
            var projectPath = GetProjectPath(projectId.ToString());
            var projectZipPath = GetProjectZipFilePath(projectPath);
            var previewPath = await dataLayer_.ConvertToNativePath(GetPreviewPath(projectId.ToString(), previewId));
            var previewFiles = await GetAllFilesRecursive(previewPath);

            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var previewFile in previewFiles)
                    {
                        var previewFilePath = previewFile.Replace(previewPath, "").Replace("\\project", "project").Replace("/project", "project");
                        if (!(await dataLayer_.IsDirectory(previewFile)))
                        {
                            using (var fileStream = await dataLayer_.ReadFile(previewFile))
                            {
                                var entry = zipArchive.CreateEntry(previewFilePath);
                                using (var zipStream = entry.Open())
                                {
                                    await fileStream.CopyToAsync(zipStream);
                                }
                            }
                        }
                        else
                        {
                            var entry = zipArchive.CreateEntry(previewFilePath);
                        }
                    }
                }

                memoryStream.Position = 0;
                await dataLayer_.CreateFile(projectZipPath, memoryStream);
            }
        }

        public async Task PackagePreviewJSON(Guid projectId, Dictionary<string, PreviewItem> previewFiles)
        {
            var projectPath = GetProjectPath(projectId.ToString());
            var projectJsonPath = GetProjectJsonFilePath(projectPath);
            JObject output = new JObject();

            foreach (var file in previewFiles)
            {
                var filePath = file.Key;
                if (file.Value.isFolder)
                {
                    filePath += "/";
                }

                output[filePath] = file.Value.stringBuilder.ToString();
            }

            using (var stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream, encoding: Encoding.UTF8, bufferSize: 8092, leaveOpen: true))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        await output.WriteToAsync(jsonWriter);
                        await jsonWriter.FlushAsync();
                    }
                }
                stream.Position = 0;
                await dataLayer_.CreateFile(projectJsonPath, stream);
            }
        }
        #endregion

        #region Thumbnail
        public async Task CreateTutorialThumbnail(Guid tutorialId, Stream thumbnail)
        {
            var projectDirectoryPath = GetProjectPath(tutorialId.ToString());
            var thumbnailFilePath = GetThumbnailFilePath(projectDirectoryPath);

            await dataLayer_.CreateDirectory(projectDirectoryPath);
            await dataLayer_.CreateFile(thumbnailFilePath, thumbnail);
        }
        #endregion
    }
}
