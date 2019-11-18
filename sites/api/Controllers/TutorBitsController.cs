using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    public class TutorBitsController : ControllerBase
    {
        private string userName_;
        protected string UserName
        {
            get
            {
                if (!useAWS)
                {
                    return "Local";
                }

                if (string.IsNullOrWhiteSpace(userName_))
                {
                    userName_ = this.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
                }
                return userName_;
            }
        }

        protected readonly IConfiguration configuration_;
        protected readonly bool useAWS;
        protected readonly bool localAdmin;

        public TutorBitsController(IConfiguration configuration)
        {
            configuration_ = configuration;
            useAWS = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<bool>(Constants.Configuration.Sections.Settings.UseAWSKey, false);

            localAdmin = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<bool>(Constants.Configuration.Sections.Settings.LocalAdminKey, false);
        }

        protected bool IsAdmin()
        {
            return useAWS ? this.User.HasClaim(c => c.Type == "cognito:groups" &&
                                            c.Value == "Admin") : localAdmin;
        }
    }
}