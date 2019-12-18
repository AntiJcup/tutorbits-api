using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda;
using LambdaModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.LambdaAccess;
using Utils.Common;
using Newtonsoft.Json;
using Amazon.ElasticTranscoder;
using Amazon.ElasticTranscoder.Model;
using System.Threading;
using Amazon.Lambda.Model;

namespace TutorBits.Lambda.AWSLambda
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAWSLambdaAccessLayer(this IServiceCollection services)
        {
            services.AddTransient<LambdaLayerInterface, AWSLambdaLayerInterface>();
            return services.AddTransient<LambdaAccessService>();
        }
    }

    public class AWSLambdaLayerInterface : LambdaLayerInterface
    {
        private readonly IConfiguration configuration_;
        private readonly IAmazonLambda lambdaClient_;
        IAmazonElasticTranscoder transcoderClient_;

        public readonly string BucketName;
        public readonly string TranscoderPresetId;
        public readonly string TranscoderPipelineId;
        public readonly string FinalizeProjectLambdaName;

        public readonly string NormalizeVideoLambdaName;

        private readonly System.TimeSpan MaxWaitForTranscode = new System.TimeSpan(hours: 0, minutes: 5, seconds: 0);

        public AWSLambdaLayerInterface(IConfiguration config, IAmazonLambda lambdaClient, IAmazonElasticTranscoder transcoderClient)
        {
            configuration_ = config;
            lambdaClient_ = lambdaClient;
            transcoderClient_ = transcoderClient;

            BucketName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Paths.BucketKey);
            TranscoderPresetId = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Settings.TranscoderPresetIdKey);
            TranscoderPipelineId = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Settings.TranscoderPipelineIdKey);
            FinalizeProjectLambdaName = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Settings.FinalizeProjectLambdaNameKey);
            NormalizeVideoLambdaName = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Settings.NormalizeVideoLambdaNameKey);
        }

        public async Task ConvertWebmToMp4(string webmPath, string outMp4Path)
        {
            WebmToMp4Request payloadModel = new WebmToMp4Request()
            {
                BucketName = BucketName,
                WebmPath = webmPath,
                Mp4Path = outMp4Path
            };
            InvokeRequest request = new InvokeRequest() { FunctionName = NormalizeVideoLambdaName, Payload = JsonConvert.SerializeObject(payloadModel) };
            var respone = await lambdaClient_.InvokeAsync(request);
        }

        //Deprecated method since I couldnt normalize the audio.
        public async Task ConvertWebmToMp4Transcoder(string webmPath, string outMp4Path)
        {
            //Tried lambda here but the lambda environment corrupts the video when using ffmpeg.
            var createJobRequest = new CreateJobRequest()
            {
                Input = new JobInput()
                {
                    AspectRatio = "auto",
                    Container = "auto",
                    FrameRate = "auto",
                    Interlaced = "auto",
                    Resolution = "auto",
                    Key = webmPath,
                },
                Output = new CreateJobOutput()
                {
                    Key = outMp4Path,
                    PresetId = TranscoderPresetId,
                },
                PipelineId = TranscoderPipelineId
            };

            var createResponse = await transcoderClient_.CreateJobAsync(createJobRequest);

            var readJobRequest = new ReadJobRequest()
            {
                Id = createResponse.Job.Id
            };

            var timeStart = DateTime.Now;
            var now = timeStart;
            while (now - timeStart < MaxWaitForTranscode)
            {
                var readResponse = await transcoderClient_.ReadJobAsync(readJobRequest);

                switch (readResponse.Job.Status)
                {
                    case "Complete":
                    case "Canceled":
                    case "Error":
                        return;
                    default:
                        break;
                }
                Thread.Sleep(100);
            }
        }

        public async Task SaveCompletedPreview(Guid projectId)
        {
            InvokeRequest request = new InvokeRequest() { FunctionName = FinalizeProjectLambdaName };
            await lambdaClient_.InvokeAsync(request);
        }
    }
}
