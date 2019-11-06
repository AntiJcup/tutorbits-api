using System.Threading.Tasks;

namespace TutorBits.LambdaAccess
{
    public interface LambdaLayerInterface
    {
        Task ConvertWebmToMp4(string webmPath, string outMp4Path);
    }
}