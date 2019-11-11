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
        private readonly string WebmToMp4Function = "WebmToMp4";
        private readonly IConfiguration configuration_;
        private readonly IAmazonLambda lambdaClient_;

        public readonly string BucketName;

        public AWSLambdaLayerInterface(IConfiguration config, IAmazonLambda lambdaClient)
        {
            configuration_ = config;
            lambdaClient_ = lambdaClient;

            BucketName = configuration_.GetSection(Constants.Configuration.Sections.PathsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Paths.BucketKey);
        }

        public async Task ConvertWebmToMp4(string webmPath, string outMp4Path)
        {
            var requestModel = new WebmToMp4Request()
            {
                BucketName = BucketName,
                Endpoint = lambdaClient_.Config.RegionEndpoint,
                WebmPath = webmPath,
                Mp4Path = outMp4Path
            };

            var lambdaExecuteRequest = new Amazon.Lambda.Model.InvokeRequest()
            {
                FunctionName = WebmToMp4Function,
                InvocationType = InvocationType.RequestResponse,
                LogType = LogType.Tail,
                Payload = JsonConvert.SerializeObject(requestModel)
            };

            await lambdaClient_.InvokeAsync(lambdaExecuteRequest);
        }
    }
}
