using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class TutorBitsController : ControllerBase
    {
        private string userName_;
        protected string UserName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(userName_))
                {
                    userName_ = this.User.Claims.FirstOrDefault(c => c.Type == "username").Value;
                }
                return userName_;
            }
        }
    }
}