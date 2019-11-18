using System;
using System.Threading.Tasks;
using TutorBits.Models.Common;

namespace TutorBits.AuthAccess
{
    public class AuthAccessService
    {
        private readonly AuthLayerInterface authLayer_;

        public AuthAccessService(AuthLayerInterface authLayer)
        {
            authLayer_ = authLayer;
        }

        public async Task<User> GetUser(string userName)
        {
            return await authLayer_.GetUser(userName);
        }
    }
}
