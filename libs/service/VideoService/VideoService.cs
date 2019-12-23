using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;

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
        private readonly FileDataAccessService fileDataService_;

        private readonly LambdaAccessService lambdaService_;

        private readonly TimeSpan TranscodeTimeout_;

        VideoService(IConfiguration configuration, FileDataAccessService fileDataService, LambdaAccessService lambdaService)
        {
            fileDataService_ = fileDataService;
            lambdaService_ = lambdaService;

            TranscodeTimeout_ = TimeSpan.FromSeconds(configuration.GetSection(Constants.Configuration.Sections.PathsKey)
                .GetValue<int>(Constants.Configuration.Sections.Paths.ProjectsDirKey));
        }

        public async Task<bool> ConvertWebmToMp4(Guid projectId)
        {
            var videoDirectory = fileDataService_.GetVideoPath(projectId.ToString());
            var webmPath = fileDataService_.GetVideoFilePath(videoDirectory);
            var mp4Path = Path.ChangeExtension(webmPath, ".mp4");

            return await lambdaService_.ConvertProjectVideo(webmPath, mp4Path);
        }

        public async Task<string> StartTranscoding(Guid projectId)
        {
            var videoDirectory = fileDataService_.GetVideoPath(projectId.ToString());
            var webmPath = fileDataService_.GetVideoFilePath(videoDirectory);
            var transcodeMp4Path = Path.ChangeExtension(webmPath, ".transcoded_mp4");
            var mp4Path = Path.ChangeExtension(webmPath, ".mp4");

            await CancelTranscoding(projectId); //Cancel if running already

            await fileDataService_.CreateTranscodingStateFile(projectId, new TranscodingStateFile()
            {
                ProjectId = projectId,
                State = TranscodingState.Transcoding,
                TranscodingOutputPath = transcodeMp4Path,
                NormalizeOutputPath = mp4Path,
                Start = DateTimeOffset.Now
            });

            var transactionJobId = await lambdaService_.ConvertProjectVideoTranscode(webmPath, mp4Path);

            return transactionJobId;
        }

        public async Task CancelTranscoding(Guid projectId)
        {
            var transcodingFileData = await fileDataService_.ReadTranscodingStateFile(projectId);

            switch (transcodingFileData.State) //End states just bail dont consider cancel
            {
                case TranscodingState.Cancelled:
                case TranscodingState.Finished:
                case TranscodingState.Errored:
                case TranscodingState.Timeout:
                    return;
            }

            transcodingFileData.State = TranscodingState.Cancelled;
            await fileDataService_.UpdateTranscodingStateFile(projectId, transcodingFileData);
        }

        public async Task<TranscodingState> CheckTranscodingStatus(Guid projectId)
        {
            var transcodingFileData = await fileDataService_.ReadTranscodingStateFile(projectId);

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
                await fileDataService_.UpdateTranscodingStateFile(projectId, transcodingFileData);
            }

            return transcodingFileData.State;
        }
    }
}

