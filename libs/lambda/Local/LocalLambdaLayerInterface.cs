using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.LambdaAccess;
using Utils;

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
    }
}
