using System;
using System.Threading.Tasks;

namespace TutorBits.LambdaAccess
{
    public interface LambdaLayerInterface
    {
        Task<bool> ConvertWebmToMp4(string webmPath, string outMp4Path);
        Task SaveCompletedPreview(Guid projectId);
        Task<bool> HealthCheck();
    }
}