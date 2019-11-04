using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/project/preview/[action]")]
    [ApiController]
    public class ProjectPreviewController : ControllerBase
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        private readonly IConfiguration configuration_;

        public ProjectPreviewController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            configuration_ = configuration;
        }

        [ActionName("create")]
        [HttpGet]
        public async Task<IActionResult> CreatePreview([FromQuery]Guid projectId, [FromQuery]uint offsetEnd)
        {
            try
            {
                var previewId = Guid.NewGuid().ToString();
                var project = await fileDataAccessService_.GetProject(projectId);
                if (project == null)
                {
                    return BadRequest();
                }
                await fileDataAccessService_.GeneratePreview(project, (int)offsetEnd, previewId);
                return new JsonResult(Utils.ProjectUrlGenerator.GenerateProjectPreviewUrl(previewId, projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
