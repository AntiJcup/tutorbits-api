using System.Threading.Tasks;
using api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.AuthAccess;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : TutorBitsController
    {
        private readonly AuthAccessService authService_;

        public UserController(IConfiguration configuration, AuthAccessService authService) : base(configuration)
        {
            authService_ = authService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await authService_.GetUser(AccessToken);

            return new JsonResult(user);
        }
    }
}
