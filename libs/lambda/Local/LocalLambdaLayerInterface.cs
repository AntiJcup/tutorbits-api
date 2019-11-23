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

        public async Task ConvertWebmToMp4(string webmPath, string outMp4Path)
        {
            var process = new Process();
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.FileName = Path.Combine(workingDirectory, ffmpegPath);
            process.StartInfo.Arguments = $"-y -i \"{webmPath}\" -crf 26 \"{outMp4Path}\"";
            process.Start();
            await process.WaitForExitAsync();
        }

        public async Task SaveCompletedPreview(Guid projectId)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false)
                .Build();
            var dataLayer = new FileDataAccessService(config, new WindowsFileDataLayerInterface());
            var project = await dataLayer.GetProject(projectId);
            var previewId = Guid.NewGuid().ToString();
            await dataLayer.GeneratePreview(project, (int)project.Duration, previewId);
            await dataLayer.PackagePreviewZIP(projectId, previewId);
        }
    }
}
