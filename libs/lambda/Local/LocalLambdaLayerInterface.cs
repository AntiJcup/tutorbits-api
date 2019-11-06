using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.LambdaAccess;

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
        public async Task ConvertWebmToMp4(string webmPath, string outMp4Path)
        {
            
        }
    }
}
