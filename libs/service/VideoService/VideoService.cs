using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.Models.Common;
using TutorBits.Project;

namespace TutorBits.Video
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddVideoService(this IServiceCollection services)
        {
            return services.AddTransient<VideoService>();
        }
    }

    public class VideoService
    {
        private readonly FileDataLayerInterface fileDataLayer_;

        private readonly LambdaLayerInterface lambdaLayer_;

        private readonly ProjectService projectService_;

        private readonly TimeSpan TranscodeTimeout_;
        public readonly string VideoDir;
        public readonly string VideoFileName;
        public readonly string TranscodeStateFileName;
        public readonly string TranscodeOutputBucket;

        public VideoService(IConfiguration configuration, FileDataLayerInterface fileDataLayer, LambdaLayerInterface lambdaLayer, ProjectService projectService)
        {
            fileDataLayer_ = fileDataLayer;
            lambdaLayer_ = lambdaLayer;
            projectService_ = projectService;

            TranscodeTimeout_ = TimeSpan.FromSeconds(configuration.GetSection(Constants.Configuration.Sections.SettingsKey)
                .GetValue<int>(Constants.Configuration.Sections.Settings.TranscodeTimeoutSecondsKey));

            VideoDir = configuration.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.VideoDirKey);

            VideoFileName = configuration.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.VideoFileNameKey);

            TranscodeStateFileName = configuration.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.TranscodeStateFileNameKey);

            TranscodeOutputBucket = configuration.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<string>(Constants.Configuration.Sections.Paths.TranscodeOutputBucketKey);
        }

        public string GetVideoPath(string projectId)
        {
            return fileDataLayer_.SanitizePath(Path.Combine(projectService_.GetProjectPath(projectId), VideoDir));
        }

        public string GetVideoFilePath(string directory)
        {
            return fileDataLayer_.SanitizePath(Path.Combine(directory, VideoFileName));
        }

        public string GetTranscodeStateFilePath(Guid projectId)
        {
            var videoDirectory = GetVideoPath(projectId.ToString());
            var videoTranscodingPath = Path.Combine(videoDirectory, TranscodeStateFileName);

            return fileDataLayer_.SanitizePath(videoTranscodingPath);
        }

        public async Task<string> StartTranscoding(Guid projectId)
        {
            var videoDirectory = GetVideoPath(projectId.ToString());
            var webmPath = GetVideoFilePath(videoDirectory);
            var mp4Path = Path.ChangeExtension(webmPath, ".mp4");

            try
            {
                await CancelTranscoding(projectId); //Cancel if running already
            }
            catch (Exception) { }

            await CreateTranscodingStateFile(projectId, new TranscodingStateFile()
            {
                ProjectId = projectId,
                State = TranscodingState.Transcoding,
                TranscodingOutputPath = mp4Path,
                NormalizeOutputPath = mp4Path,
                Start = DateTimeOffset.Now
            });

            await DeletePreviousTranscode(projectId);
            var transactionJobId = await lambdaLayer_.ConvertWebmToMp4Transcoder(webmPath, mp4Path);

            return transactionJobId;
        }

        public async Task CancelTranscoding(Guid projectId)
        {
            var transcodingFileData = await ReadTranscodingStateFile(projectId);

            switch (transcodingFileData.State) //End states just bail dont consider cancel
            {
                case TranscodingState.Cancelled:
                case TranscodingState.Finished:
                case TranscodingState.Errored:
                case TranscodingState.Timeout:
                    return;
            }

            transcodingFileData.State = TranscodingState.Cancelled;
            await UpdateTranscodingStateFile(projectId, transcodingFileData);
        }

        public async Task<TranscodingState> CheckTranscodingStatus(Guid projectId)
        {
            var transcodingFileData = await ReadTranscodingStateFile(projectId);

            switch (transcodingFileData.State) //End states just bail dont consider timeout
            {
                case TranscodingState.Cancelled:
                case TranscodingState.Finished:
                case TranscodingState.Errored:
                case TranscodingState.Timeout:
                    return transcodingFileData.State;
            }

            if (DateTimeOffset.Now - transcodingFileData.Start > TranscodeTimeout_)
            {
                transcodingFileData.State = TranscodingState.Timeout;
                await UpdateTranscodingStateFile(projectId, transcodingFileData);
            }

            return transcodingFileData.State;
        }

        public async Task<string> StartVideoRecording(Guid projectId)
        {
            var videoPath = GetVideoPath(projectId.ToString());
            var videoFilePath = GetVideoFilePath(videoPath);
            var uploadId = await fileDataLayer_.StartMultipartUpload(videoFilePath);
            return uploadId;
        }

        public async Task<string> ContinueVideoRecording(Guid projectId, string uploadId, int part, Stream videoPart, bool last)
        {
            var videoPath = GetVideoPath(projectId.ToString());
            var videoFilePath = GetVideoFilePath(videoPath);
            return await fileDataLayer_.UploadPart(videoFilePath, uploadId, part, videoPart, last);
        }

        public async Task<string> FinishVideoRecording(Guid projectId, string uploadId, ICollection<VideoPart> parts)
        {
            var videoPath = GetVideoPath(projectId.ToString());
            var videoFilePath = GetVideoFilePath(videoPath);
            return await fileDataLayer_.StopMultipartUpload(videoFilePath, uploadId, parts);
        }

        public async Task CreateTranscodingStateFile(Guid projectId, TranscodingStateFile data)
        {
            var transcodingStateFilePath = GetTranscodeStateFilePath(projectId);

            using (var dataStream = new MemoryStream())
            {
                Utils.Common.JsonUtils.Serialize(data, dataStream);
                dataStream.Position = 0;
                await fileDataLayer_.CreateFile(transcodingStateFilePath, dataStream);
            }
        }

        public async Task<TranscodingStateFile> ReadTranscodingStateFile(Guid projectId)
        {
            var transcodingStateFilePath = GetTranscodeStateFilePath(projectId);
            return Utils.Common.JsonUtils.Deserialize<TranscodingStateFile>(await fileDataLayer_.ReadFile(transcodingStateFilePath));
        }

        public async Task UpdateTranscodingStateFile(Guid projectId, TranscodingStateFile data)
        {
            var transcodingStateFilePath = GetTranscodeStateFilePath(projectId);

            if (!(await fileDataLayer_.FileExists(transcodingStateFilePath)))
            {
                return;
            }

            using (var dataStream = new MemoryStream())
            {
                Utils.Common.JsonUtils.Serialize(data, dataStream);
                dataStream.Position = 0;
                await fileDataLayer_.UpdateFile(transcodingStateFilePath, dataStream);
            }
        }

        public async Task DeletePreviousTranscode(Guid projectId)
        {
            var videoDirectory = GetVideoPath(projectId.ToString());
            var videoPath = Path.ChangeExtension(GetVideoFilePath(videoDirectory), ".mp4");

            if (await fileDataLayer_.FileExists(videoPath, TranscodeOutputBucket))
            {
                await fileDataLayer_.DeleteFile(videoPath, TranscodeOutputBucket);
            }
        }
    }
}

