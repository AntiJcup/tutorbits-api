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

        public readonly string HealthCheckLambdaName;

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
            HealthCheckLambdaName = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Settings.HealthCheckLambdaNameKey);
        }

        public async Task<bool> ConvertWebmToMp4(string webmPath, string outMp4Path)
        {
            WebmToMp4Request payloadModel = new WebmToMp4Request()
            {
                BucketName = BucketName,
                WebmPath = webmPath,
                Mp4Path = outMp4Path
            };
            InvokeRequest request = new InvokeRequest() { FunctionName = NormalizeVideoLambdaName, Payload = JsonConvert.SerializeObject(payloadModel) };
            var response = await lambdaClient_.InvokeAsync(request);
            Console.WriteLine(response.FunctionError);
            return string.IsNullOrWhiteSpace(response.FunctionError);
        }

        //Returns job id
        public async Task<string> ConvertWebmToMp4Transcoder(string webmPath, string outMp4Path)
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

            return createResponse.Job.Id;
        }

        public async Task SaveCompletedPreview(Guid projectId)
        {
            InvokeRequest request = new InvokeRequest() { FunctionName = FinalizeProjectLambdaName };
            await lambdaClient_.InvokeAsync(request);
        }

        public async Task<bool> HealthCheck()
        {
            InvokeRequest request = new InvokeRequest() { FunctionName = HealthCheckLambdaName };
            var response = await lambdaClient_.InvokeAsync(request);
            Console.WriteLine(response.FunctionError);
            return string.IsNullOrWhiteSpace(response.FunctionError);
        }
    }
}
