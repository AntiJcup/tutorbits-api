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
using System;
using System.Collections;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AudioNormalize
{
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }

    //Due to an environment issue with lambda c# on aws lambda FFMPEG corrupts everything it touches (TRY AGAIN when new environment is rolled)
    /*
        Things ive tried:
            * Building my own ffmpeg on Amazon Linux ec2
            * Using the node compatible ffmpeg layer
            * Downloading the linux supported ffmpeg and shipping with it
            * Running ffmpeg from bash
            * Running ffmpeg directly
            * Tried changing the working directory to ffmpegs folder
            * Googling the errors that came back 
    */
    public class Function
    {
        private static readonly string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string tmpDirectory = "/tmp";
        private static readonly string ffmpegPath = "/opt/bin/ffmpeg";

        /// <summary>
        /// Normalize volume on mp4
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<bool> FunctionHandler(Stream input, ILambdaContext context)
        {
            try
            {
                NormalizeVolumeOnMp4Request inputModel = null;
                using (StreamReader sr = new StreamReader(input))
                {
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        inputModel = serializer.Deserialize<NormalizeVolumeOnMp4Request>(reader);
                    }
                }

                var localMp4File = Path.ChangeExtension(Path.Combine(tmpDirectory, $"{Path.GetTempFileName()}"), ".mp4");
                var quietLocalMp4File = Path.ChangeExtension(Path.Combine(tmpDirectory, $"{Path.GetTempFileName()}"), ".mp4");

                var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(inputModel.Endpoint));

                //Get mp4 file
                var webmRequest = new GetObjectRequest()
                {
                    BucketName = inputModel.BucketName,
                    Key = inputModel.Mp4Path
                };

                Console.WriteLine("Downloading mp4");
                var webmResponse = await s3Client.GetObjectAsync(webmRequest);
                using (var webmStream = webmResponse.ResponseStream)
                {
                    using (var localMp4FileStream = File.Create(localMp4File))
                    {
                        await webmStream.CopyToAsync(localMp4FileStream);
                        await localMp4FileStream.FlushAsync();
                    }
                }
                Console.WriteLine($"Finished downloading mp4 {(new FileInfo(localMp4File)).Length}");

                //Normalize mp4
                Console.WriteLine($"Normalizing {localMp4File} to {quietLocalMp4File}");

                Console.WriteLine($"cd {tmpDirectory} && ls /opt".Bash());
                Console.WriteLine($"cd {tmpDirectory} && ls /opt/bin".Bash());
                Console.WriteLine($"cd {tmpDirectory} && {ffmpegPath} -loglevel error -i {localMp4File} -af \"highpass=200, lowpass=2000\" {quietLocalMp4File}".Bash());
                Console.WriteLine("Finished normalizing mp4");

                var mp4Request = new PutObjectRequest()
                {
                    BucketName = inputModel.BucketName,
                    Key = inputModel.Mp4Path + ".bak",
                    InputStream = File.OpenRead(quietLocalMp4File)
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
