using System;
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

        public LambdaAccessService(IConfiguration configuration, LambdaLayerInterface lambdaLayer)
        {
            configuration_ = configuration;
            lambdaLayer_ = lambdaLayer;
        }

        public async Task<bool> ConvertProjectVideo(string webmPath, string mp4Path)
        {
            return await lambdaLayer_.ConvertWebmToMp4(webmPath, mp4Path);
        }

        public async Task<string> ConvertProjectVideoTranscode(string webmPath, string mp4Path)
        {
            return await lambdaLayer_.ConvertWebmToMp4Transcoder(webmPath, mp4Path);
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