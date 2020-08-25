using System.Threading.Tasks;
using TutorBits.Models.Common;

namespace TutorBits.AuthAccess
{
    public interface AuthLayerInterface
    {
        Task<User> GetUser(string accountToken);
    }
}
