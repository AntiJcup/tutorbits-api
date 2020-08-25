using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tracer;
using TutorBits.FileDataAccess;
using TutorBits.Project;

namespace TutorBits.Preview
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPreviewService(this IServiceCollection services)
        {
            return services.AddTransient<PreviewService>();
        }
    }

    public class PreviewService
    {
        private readonly FileDataLayerInterface dataLayer_;

        private readonly IConfiguration configuration_;

        private readonly ProjectService projectService_;

        public readonly string PreviewsDir;
        public readonly string TempDirectory;
        public readonly string ProjectZipName;
        public readonly string ProjectJsonName;
        public readonly string PreviewHelpersBucket;
        public readonly string PreviewHelpersPath;

        public PreviewService(IConfiguration configuration, FileDataLayerInterface dataLayer, ProjectService projectService)
        {
            configuration_ = configuration;
            dataLayer_ = dataLayer;
            projectService_ = projectService;

            PreviewsDir = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.PreviewsDirKey);

            ProjectZipName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectZipNameKey);
            ProjectJsonName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.ProjectJsonNameKey);

            PreviewHelpersBucket = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.PreviewHelpersBucketKey);
            PreviewHelpersPath = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.PreviewHelpersPathKey);

            TempDirectory = Path.GetTempPath();
        }

        public string GetPreviewPath(string projectId, string previewId)
        {
            return dataLayer_.SanitizePath(Path.Combine(PreviewsDir, projectId, previewId));
        }

        public string GetProjectZipFilePath(string directory)
        {
            return dataLayer_.SanitizePath(Path.Combine(directory, ProjectZipName));
        }

        public string GetProjectJsonFilePath(string directory)
        {
            return dataLayer_.SanitizePath(Path.Combine(directory, ProjectJsonName));
        }

        public void GeneratePreviewForTransactionLog(string projectId, TraceTransactionLog transactionLog, int end, Dictionary<string, PreviewItem> files)
        {
            foreach (var transaction in transactionLog.Transactions)
            {
                if (transaction.TimeOffsetMs > end)
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
                    case TraceTransaction.Types.TraceTransactionType.UploadFile:
                        var uploadFileData = transaction.UploadFile;
                        var resourceDirectory = projectService_.GetProjectResourceDir(projectService_.GetProjectPath(projectId));
                        files[uploadFileData.NewFilePath] = new PreviewItem
                        {
                            resourcePath = projectService_.GetProjectResourceFilePath(resourceDirectory, uploadFileData.ResourceId),
                            stringBuilder = new StringBuilder(),
                            resourceId = uploadFileData.ResourceId
                        };
                        break;
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
                        var deleteFileData = transaction.DeleteFile;
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
                if (string.IsNullOrWhiteSpace(file.Key) || file.Key == "/project")
                {
                    continue;
                }
                var fullPath = (await dataLayer_.ConvertToNativePath(dataLayer_.SanitizePath(Path.Combine(previewFolder, file.Key.Substring(1)))));
                await dataLayer_.CreatePathForFile(fullPath);
                var fileBytes = Encoding.UTF8.GetBytes(file.Value.stringBuilder.ToString());
                using (var stream = new MemoryStream(fileBytes))
                {
                    if (!string.IsNullOrWhiteSpace(file.Value.resourcePath))
                    {
                        await dataLayer_.CopyFile(file.Value.resourcePath, fullPath);
                    }
                    else if (file.Value.isFolder)
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

        public async Task<Dictionary<string, PreviewItem>> LoadProjectPreviewJson(Guid projectId)
        {
            var projectPath = projectService_.GetProjectPath(projectId.ToString());
            var projectJsonPath = GetProjectJsonFilePath(projectPath);
            JObject input = null;

            using (var stream = await dataLayer_.ReadFile(projectJsonPath))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    using (JsonTextReader reader = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();
                        input = serializer.Deserialize<JObject>(reader);
                    }
                }
            }

            if (input == null)
            {
                return null;
            }

            var output = new Dictionary<string, PreviewItem>();
            var resourceDirectory = projectService_.GetProjectResourceDir(projectPath);
            foreach (var item in input)
            {
                string key = item.Key;
                var previewItem = new PreviewItem();
                previewItem.isFolder = item.Key.EndsWith("/");
                previewItem.stringBuilder = new StringBuilder();
                if (!previewItem.isFolder && item.Key.StartsWith("res:"))
                {
                    key = key.Replace("res:", "");
                    previewItem.resourceId = item.Value.ToString();
                    previewItem.resourcePath = projectService_.GetProjectResourceFilePath(resourceDirectory, previewItem.resourceId);
                }
                else
                {
                    key = previewItem.isFolder ? item.Key.Substring(0, item.Key.Count() - 1) : key;

                    previewItem.stringBuilder.Insert(0, item.Value);
                }
                output[key] = previewItem;
            }

            return output;
        }


        public async Task<Dictionary<string, PreviewItem>> GeneratePreview(TraceProject project, int end, string previewId, bool includePreviewHelpers)
        {
            var projectId = Guid.Parse(project.Id);
            var previewPath = GetPreviewPath(project.Id, previewId);

            var transactionLogPaths = await projectService_.GetTransactionLogsForRange(projectId, 0, (uint)end);
            var files = new Dictionary<string, PreviewItem>();
            foreach (var transactionLogPath in transactionLogPaths.OrderBy(p => int.Parse(p.Key)))
            {
                using (var transactionLogStream = await dataLayer_.ReadFile(transactionLogPath.Value))
                {
                    var transactionLog = TraceTransactionLog.Parser.ParseFrom(transactionLogStream);
                    GeneratePreviewForTransactionLog(project.Id, transactionLog, end, files);
                }
            }

            if (includePreviewHelpers)
            {
                await IncludePreviewHelpers(project, previewId);
            }

            await SavePreviewCache(files, previewPath);

            return files;
        }

        public async Task GeneratePreview(TraceProject project, int end, string previewId, TraceTransactionLogs traceTransactionLogs,
                                            bool includePreviewHelpers, Guid? baseProjectId = null)
        {
            var projectId = Guid.Parse(project.Id);
            var previewPath = GetPreviewPath(project.Id, previewId);
            var files = baseProjectId.HasValue ? (await LoadProjectPreviewJson(baseProjectId.Value)) : new Dictionary<string, PreviewItem>();
            foreach (var transactionLog in traceTransactionLogs.Logs)
            {
                GeneratePreviewForTransactionLog(project.Id, transactionLog, end, files);
            }

            if (includePreviewHelpers)
            {
                await IncludePreviewHelpers(project, previewId);
            }

            await SavePreviewCache(files, previewPath);
        }

        public async Task IncludeJSPreviewHelper(string previewPath)
        {
            var sourceJSHelperPath = dataLayer_.SanitizePath(Path.Combine(PreviewHelpersPath, "js"));
            var destinationJSHelperPath = dataLayer_.SanitizePath(Path.Combine(dataLayer_.GetWorkingDirectory(), previewPath, "preview-helpers/js"));
            await dataLayer_.CreateDirectory(destinationJSHelperPath);
            await dataLayer_.CopyDirectory(sourceJSHelperPath, destinationJSHelperPath, PreviewHelpersBucket);
        }

        public async Task IncludePYPreviewHelper(string previewPath)
        {
            var sourcePYHelperPath = dataLayer_.SanitizePath(Path.Combine(PreviewHelpersPath, "py"));
            var destinationPYHelperPath = dataLayer_.SanitizePath(Path.Combine(dataLayer_.GetWorkingDirectory(), previewPath, "preview-helpers/py"));
            await dataLayer_.CreateDirectory(destinationPYHelperPath);
            await dataLayer_.CopyDirectory(sourcePYHelperPath, destinationPYHelperPath, PreviewHelpersBucket);
        }

        public async Task IncludePreviewHelpers(TraceProject project, string previewId)
        {
            var previewPath = GetPreviewPath(project.Id, previewId);
            await IncludeJSPreviewHelper(previewPath);
            await IncludePYPreviewHelper(previewPath);
        }

        public async Task PackagePreviewZIP(string previewPath, string outputZipPath)
        {
            var previewFiles = await dataLayer_.GetAllFiles(previewPath);
            var tempZipPath = Path.Combine(TempDirectory, Guid.NewGuid().ToString());

            using (var outFileStream = File.Create(tempZipPath))
            {
                using (var zipArchive = new ZipArchive(outFileStream, ZipArchiveMode.Create, true))
                {
                    foreach (var previewFile in previewFiles)
                    {
                        var previewFilePath = previewFile.Replace(previewPath, "").Replace("\\project", "project").Replace("/project", "project");
                        if (!(await dataLayer_.IsDirectory(previewFile)))
                        {
                            using (var resourceFileStream = await dataLayer_.ReadFile(previewFile))
                            {
                                var entry = zipArchive.CreateEntry(previewFilePath);
                                using (var zipStream = entry.Open())
                                {
                                    await resourceFileStream.CopyToAsync(zipStream);
                                }
                            }
                        }
                        else
                        {
                            // var entry = zipArchive.CreateEntry(previewFilePath); Adds an extra file
                        }
                    }
                }
            }

            using (var zipFileStream = File.OpenRead(tempZipPath))
            {
                await dataLayer_.CreatePathForFile(outputZipPath);
                await dataLayer_.CreateFile(outputZipPath, zipFileStream);
            }
        }

        public async Task PackagePreviewZIP(Guid projectId, string previewId)
        {
            var projectPath = projectService_.GetProjectPath(projectId.ToString());
            var projectZipPath = GetProjectZipFilePath(projectPath);
            var previewPath = await dataLayer_.ConvertToNativePath(GetPreviewPath(projectId.ToString(), previewId));

            await PackagePreviewZIP(previewPath, projectZipPath);
        }

        public async Task<Stream> DownloadPreview(Guid projectId, string previewId)
        {
            var projectPath = projectService_.GetProjectPath(projectId.ToString());
            var projectZipPath = GetProjectZipFilePath(projectPath);
            var previewPath = await dataLayer_.ConvertToNativePath(GetPreviewPath(projectId.ToString(), previewId));
            await PackagePreviewZIP(previewPath, projectZipPath);

            return await dataLayer_.ReadFile(projectZipPath);
        }

        public async Task PackagePreviewJSON(string outputJsonPath, Dictionary<string, PreviewItem> previewFiles)
        {
            JObject output = new JObject();

            foreach (var file in previewFiles)
            {
                var filePath = file.Key;
                if (file.Value.isFolder)
                {
                    filePath += "/";
                }

                if (!string.IsNullOrWhiteSpace(file.Value.resourcePath))
                {
                    filePath = "res:" + filePath;
                    output[filePath] = file.Value.resourceId;
                }
                else
                {
                    output[filePath] = file.Value.stringBuilder.ToString();
                }
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
                await dataLayer_.CreateFile(outputJsonPath, stream);
            }
        }

        public async Task PackagePreviewJSON(Guid projectId, Dictionary<string, PreviewItem> previewFiles)
        {
            var projectPath = projectService_.GetProjectPath(projectId.ToString());
            var projectJsonPath = GetProjectJsonFilePath(projectPath);

            await PackagePreviewJSON(projectJsonPath, previewFiles);
        }
    }
}

