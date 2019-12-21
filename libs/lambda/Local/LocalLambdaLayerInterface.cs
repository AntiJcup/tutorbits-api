using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.WindowsFileSystem;
using Utils.Common;

namespace TutorBits.Lambda.Local
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLocalLambdaAccessLayer(this IServiceCollection services)
        {
            services.AddTransient<LambdaLayerInterface, LocalLambdaLayerInterface>();
            return services.AddTransient<LambdaAccessService>();
        }
    }

    public class LocalLambdaLayerInterface : LambdaLayerInterface
    {
        private static readonly string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string ffmpegPath = "ffmpeg.exe";

        public async Task<bool> ConvertWebmToMp4(string webmPath, string outMp4Path)
        {
            var convertProcess = new Process();
            convertProcess.StartInfo.WorkingDirectory = workingDirectory;
            convertProcess.StartInfo.FileName = Path.Combine(workingDirectory, ffmpegPath);
            convertProcess.StartInfo.Arguments = $"-loglevel error -y -i \"{webmPath}\" -vcodec libx264 -vprofile high -preset veryfast -threads 0 -vf scale=-1:720 -codec:a aac -strict experimental -af highpass=200,lowpass=1500,loudnorm=I=-35:TP=-1.5:LRA=20 \"{outMp4Path}\"";
            convertProcess.Start();
            await convertProcess.WaitForExitAsync();

            return File.Exists(outMp4Path);
        }

        public async Task SaveCompletedPreview(Guid projectId)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false)
                .Build();
            var dataLayer = new FileDataAccessService(config, new WindowsFileDataLayerInterface());
            var project = await dataLayer.GetProject(projectId);
            var previewId = Guid.NewGuid().ToString();
            var previewDictionary = await dataLayer.GeneratePreview(project, (int)project.Duration, previewId);
            await dataLayer.PackagePreviewZIP(projectId, previewId);
            await dataLayer.PackagePreviewJSON(projectId, previewDictionary);
        }

        public async Task<bool> HealthCheck()
        {
            //DO nothing we are local
            return true;
        }
    }
}
