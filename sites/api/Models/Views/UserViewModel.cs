using System.Linq;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;

namespace api.Models.Views
{
    public class UserViewModel
    {
        public string UserName;
        public string Email;

        public void Convert(GetUserResponse user)
        {
            UserName = user.Username;
            Email = user.UserAttributes.FirstOrDefault(a => a.Name == "email").Value;
        }
    }
}