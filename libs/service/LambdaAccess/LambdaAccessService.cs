﻿using System;
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

        public async Task ConvertProjectVideo(Guid projectId)
        {
            var videoDirectory = fileDataService_.GetVideoPath(projectId.ToString());
            var webmPath = fileDataService_.GetVideoFilePath(videoDirectory);
            var mp4Path = Path.ChangeExtension(webmPath, ".mp4");
            await lambdaLayer_.ConvertWebmToMp4(webmPath, mp4Path);
        }
    }
}