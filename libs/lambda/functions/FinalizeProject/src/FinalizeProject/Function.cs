using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Amazon.Lambda.Core;
using LambdaModels;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using TutorBits.FileDataAccess;
using TutorBits.S3FileSystem;
using Amazon.S3;
using Amazon.S3.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace FinalizeProject
{
    public class Function
    {
        /// <summary>
        /// Creates json and zip files of the project when completed
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(Stream input, ILambdaContext context)
        {
            FinalizeProjectRequest inputModel = null;
            using (StreamReader sr = new StreamReader(input))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    inputModel = serializer.Deserialize<FinalizeProjectRequest>(reader);
                }
            }

            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false)
                .Build();

            var endpoint = config.GetSection(Constants.Configuration.Sections.PathsKey)
                    .GetValue<string>(Constants.Configuration.Sections.Paths.BucketKey);
            
            var dataLayer = new FileDataAccessService(config,
                new S3FileDataLayerInterface(config, new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(endpoint))));
            var project = await dataLayer.GetProject(inputModel.ProjectId);
            var previewId = Guid.NewGuid().ToString();
            await dataLayer.GeneratePreview(project, (int)project.Duration, previewId);
            await dataLayer.PackagePreviewZIP(inputModel.ProjectId, previewId);
        }
    }
}
