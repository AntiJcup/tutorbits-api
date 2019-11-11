using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;
using Utils.Common;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using LambdaModels;
using Amazon;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace WebmToMp4
{
    public class Function
    {
        private static readonly string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string ffmpegPath = "ffmpeg";

        /// <summary>
        /// Converts webm files to mp4
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> FunctionHandler(string input, ILambdaContext context)
        {
            try
            {
                var inputModel = JsonConvert.DeserializeObject<WebmToMp4Request>(input);
                var localWebmFile = $"{Guid.NewGuid().ToString()}.webm";
                var localMp4File = $"{Guid.NewGuid().ToString()}.mp4";

                var s3Client = new AmazonS3Client(inputModel.Endpoint);

                //Get webm file
                var webmRequest = new GetObjectRequest()
                {
                    BucketName = inputModel.BucketName,
                    Key = inputModel.WebmPath
                };

                var webmResponse = await s3Client.GetObjectAsync(webmRequest);
                using (var webmStream = webmResponse.ResponseStream)
                {
                    using (var localWebmFileStream = File.Create(localWebmFile))
                    {
                        await webmStream.CopyToAsync(localWebmFileStream);
                    }
                }

                //Convert webm file to mp4
                var process = new Process();
                process.StartInfo.WorkingDirectory = workingDirectory;
                process.StartInfo.FileName = Path.Combine(workingDirectory, ffmpegPath);
                process.StartInfo.Arguments = $"-y -i \"{localWebmFile}\" -crf 26 \"{localMp4File}\"";
                process.Start();
                await process.WaitForExitAsync();

                var mp4Request = new PutObjectRequest()
                {
                    BucketName = inputModel.BucketName,
                    Key = inputModel.Mp4Path,
                    InputStream = File.OpenRead(localMp4File)
                };
                
                var mp4Response = await s3Client.PutObjectAsync(mp4Request);


                return true;
            }
            catch (Exception e)
            {
                context.Logger.LogLine(e.ToString());
            }

            return false;
        }
    }
}
