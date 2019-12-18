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
using System.Security.AccessControl;
using System.Security.Principal;
using System.Runtime.InteropServices;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace WebmToMp4
{
    public class Function
    {
        private static readonly string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string tmpDirectory = "/tmp";
        private static readonly string ffmpegPath = "/opt/ffmpeg";

        /// <summary>
        /// Converts webm files to mp4
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> FunctionHandler(Stream input, ILambdaContext context)
        {
            try
            {
                WebmToMp4Request inputModel = null;
                using (StreamReader sr = new StreamReader(input))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        inputModel = serializer.Deserialize<WebmToMp4Request>(reader);
                    }
                }

                var localWebmFile = Path.ChangeExtension(Path.Combine(tmpDirectory, $"{Path.GetTempFileName()}"), ".webm");
                var localMp4File = Path.ChangeExtension(Path.Combine(tmpDirectory, $"{Path.GetTempFileName()}"), ".mp4");

                var s3Client = new AmazonS3Client();

                //Get webm file
                var webmRequest = new GetObjectRequest()
                {
                    BucketName = inputModel.BucketName,
                    Key = inputModel.WebmPath
                };

                Console.WriteLine("Downloading webm");
                var webmResponse = await s3Client.GetObjectAsync(webmRequest);
                using (var webmStream = webmResponse.ResponseStream)
                {
                    using (var localWebmFileStream = File.Create(localWebmFile))
                    {
                        await webmStream.CopyToAsync(localWebmFileStream);
                        await localWebmFileStream.FlushAsync();
                    }
                }
                Console.WriteLine($"Finished downloading webm {(new FileInfo(localWebmFile)).Length}");

                //Convert webm file to mp4
                Console.WriteLine($"Converting {localWebmFile} to {localMp4File}");
                var process = new Process();
                process.StartInfo.WorkingDirectory = "/opt";
                process.StartInfo.FileName = ffmpegPath;
                process.StartInfo.Arguments = $"-i \"{localWebmFile}\" -vsync vfr \"{localMp4File}\"";
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                Console.WriteLine("Finished converting webm");

                var mp4Request = new PutObjectRequest()
                {
                    BucketName = inputModel.BucketName,
                    Key = inputModel.Mp4Path,
                    InputStream = File.OpenRead(localMp4File)
                };

                Console.WriteLine("Uploading mp4");
                var mp4Response = await s3Client.PutObjectAsync(mp4Request);
                Console.WriteLine("Finished uploading mp4");

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
