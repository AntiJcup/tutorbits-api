using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Models.Requests;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TutorialController : ControllerBase
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        private readonly IConfiguration configuration_;

        public TutorialController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            configuration_ = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery]CreateTutorial createTutorialRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // return ;
                }
                // return new JsonResult(Utils.ProjectUrlGenerator.GenerateProjectUrl(projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
