using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.Models.Common;

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

        protected bool IsExternalLogin
        {
            get
            {
                if (!useAWS || string.IsNullOrWhiteSpace(googleUserGroup_))
                {
                    return false;
                }

                //TODO Other external logins need to be checked here when added
                return this.User.HasClaim(c => c.Type == "cognito:groups" && (c.Value == googleUserGroup_));
            }
        }

        protected readonly IConfiguration configuration_;
        protected readonly bool useAWS;
        protected readonly bool localAdmin;

        private readonly string googleUserGroup_;

        public TutorBitsController(IConfiguration configuration)
        {
            configuration_ = configuration;
            useAWS = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<bool>(Constants.Configuration.Sections.Settings.UseAWSKey, false);

            localAdmin = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<bool>(Constants.Configuration.Sections.Settings.LocalAdminKey, false);

            googleUserGroup_ = configuration_.GetSection(Constants.Configuration.Sections.SettingsKey)
                        .GetValue<string>(Constants.Configuration.Sections.Settings.GoogleExternalGroupNameKey);
        }

        protected bool IsAdmin()
        {
            return useAWS ? this.User.HasClaim(c => c.Type == "cognito:groups" &&
                                            c.Value == "Admin") : localAdmin;
        }

        protected bool HasAccessToModel<TModel>(TModel model) where TModel : BaseModel
        {
            return model.Owner == UserName || IsAdmin();
        }
    }
}