using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.LambdaAccess;
using Utils.Common;

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
        public async Task ConvertWebmToMp4(string webmPath, string outMp4Path)
        {
            // TODO Run this lambda
        }
    }
}
