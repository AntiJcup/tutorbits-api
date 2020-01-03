using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using Utils.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/project/video/streaming/[action]")]
    [ApiController]
    public class VideoStreamingController : TutorBitsController
    {
        public VideoStreamingController(IConfiguration configuration)
         : base(configuration)
        {
        }

        [ActionName("video")]
        [HttpGet]
        public IActionResult GetVideoUrl([FromQuery]Guid projectId)
        {
            try
            {
                return new JsonResult(ProjectUrlGenerator.GenerateProjectVideoUrl(projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
