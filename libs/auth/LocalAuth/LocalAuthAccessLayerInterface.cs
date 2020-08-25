using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TutorBits.AuthAccess;
using TutorBits.Models.Common;

namespace LocalAuth
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLocalAuthAccessLayer(this IServiceCollection services)
        {
            services.AddTransient<AuthLayerInterface, LocalAuthAccessLayerInterface>();
            return services.AddTransient<AuthAccessService>();
        }
    }
    public class LocalAuthAccessLayerInterface : AuthLayerInterface
    {
        private readonly IConfiguration configuration_;
        public LocalAuthAccessLayerInterface(IConfiguration config)
        {
            configuration_ = config;
        }

        public async Task<User> GetUser(string accessToken)
        {
            return await Task.FromResult(new User()
            {
                Name = "Local",
                Email = "jbrummel7@gmail.com"
            });
        }
    }
}
