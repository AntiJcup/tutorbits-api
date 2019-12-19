using System;
using TutorBits.FileDataAccess;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IO;

namespace TutorBits.LambdaAccess
{
    public class LambdaAccessService
    {
        public readonly string ProjectsDir;
        public readonly string VideoDir;
        public readonly string VideoFileName;

        private readonly IConfiguration configuration_;

        private readonly LambdaLayerInterface lambdaLayer_;

        private readonly FileDataAccessService fileDataService_;

        public LambdaAccessService(IConfiguration configuration, LambdaLayerInterface lambdaLayer, FileDataAccessService fileDataService)
        {
            configuration_ = configuration;
            lambdaLayer_ = lambdaLayer;
            fileDataService_ = fileDataService;
        }

        public async Task<bool> ConvertProjectVideo(Guid projectId)
        {
            var videoDirectory = fileDataService_.GetVideoPath(projectId.ToString());
            var webmPath = fileDataService_.GetVideoFilePath(videoDirectory);
            var mp4Path = Path.ChangeExtension(webmPath, ".mp4");
            return await lambdaLayer_.ConvertWebmToMp4(webmPath, mp4Path);
        }

        public async Task FinalizeProject(Guid projectId)
        {
            await lambdaLayer_.SaveCompletedPreview(projectId);
        }

        public async Task<bool> HealthCheck()
        {
            return await lambdaLayer_.HealthCheck();
        }
    }
}