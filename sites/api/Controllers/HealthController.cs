using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TutorBits.Models.Common;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Check()
        {
            return Ok();
        }
    }
}